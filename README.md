# Apply Bing map background the easy way to WaterGEMS WaterCAD or WaterOPS

Add Bing map background in the main application is not the easiest as you have to figure out, copy/paste the latitude and longitude for the corresponding point(s).

This application aim on easing that process. All one has to enter is either EPSG (European Petroleum Survey Group) code or country/state of the network to lookup the EPSG code.

## Demo

[![Apply Bing Background Demo](http://img.youtube.com/vi/19C8svER84g/0.jpg)](https://youtu.be/19C8svER84g "Demo Video")

## Download

Make sure to download the right version of the application. The OpenFlows-Water (OFW) is relative new API, so newer version of Water products are currently supported

### [Download v10.4.x.x](OFW.BingBackground/_setup.bat)

## Setup (Must Do!)

After extracting the contents from the compressed file, paste them over to the installation directory (typically: `C:\Program Files (x86)\Bentley\WaterGEMS\x64`).

## How to run

Open up the `OFW.BingBackground.exe` and screen like below loads.
![model_merger_form](images/model_merger_form.png "Model Merger Form")

## Methodology

For a given network, the tool find the Control Points, triangular points based on Junction locations. The X, and Y values from these triangular points are converted to Latitude and Longitude using DotSpatial package.

Once the latitude and longitude are obtained for given triangular points, using WaterObjects.NET API, hydraulic model is updated to add the bing background layer.

### Control Points

```csharp
public double[][] GetControlPointsUsingJunctionNodes()
{
    var xField = WaterModel.Network
        .Junctions.InputFields.FieldByName(StandardFieldName.HmiGeometryXCoordinate);

    var yField = WaterModel.Network
        .Junctions.InputFields.FieldByName(StandardFieldName.HmiGeometryYCoordinate);

    var points = MathLibrary.TriangulatedControlPoints(xField, yField);
    return points;
}


public double[][] GetLatLngPoints(double[][] points, ProjectionInfo fromProj)
{
    var toProj = ProjectionInfo.FromEpsgCode(LatLngEpsgCode);
    var latLngPoints = new double[points.Length][];

    for (int i = 0; i < points.Length; i++)
    {
        latLngPoints[i] = new double[2] {points[i][0], points[i][1]};
        Reproject.ReprojectPoints(latLngPoints[i], null, fromProj, toProj, 0, 1);
    }

    return latLngPoints;
}
```

### Update Hydraulic Model

```csharp
public  void AddBingMapLayer(
            IWaterModel waterModel,
            IGraphicalProject project,
            ProjectionInfo fromProj)
{
    // Get the coordinate of control points 
    var controlPoints = new ControlPoints(waterModel);            
    var controlPointVertices = controlPoints.GetControlPointsUsingJunctionNodes();


    // Get the Lat Lng of the control points
    var controlLatLngPointVerticies = controlPoints.GetLatLngPoints(controlPointVertices, fromProj);
    

    // Define Bing Map
    var bingMapLayerData = project.BingMapsBackgroundDefinition;

    // Add Bing map layer in to the project
    bingMapLayerData.Visible = true;
    bingMapLayerData.Active = true;


    // update the bing map properties
    bingMapLayerData.ControlPointManager.NumberOfControlPoints = controlPointVertices.Length;
    bingMapLayerData.BingMapsImagerySet = (int)EnumBingMapsImagerySet.Road;
    bingMapLayerData.Visible = true;


    // Add lat, Lng and X, Y to each row
    for (int i = 0; i < controlPointVertices.Length; i++)
    {
        VirtualMapControlPointElement row = project
            .BingMapsBackgroundDefinition
            .ControlPointManager
            .Elements()[i] as VirtualMapControlPointElement;

        row.Latitude = controlLatLngPointVerticies[i][1];
        row.Longitude = controlLatLngPointVerticies[i][0];
        row.X = controlPointVertices[i][0];
        row.Y = controlPointVertices[i][1];

    }

    // initialize virtual map so that it will get loaded in the drawing
    var virtualMaps = bingMapLayerData.VirtualMaps;            
    if (!virtualMaps.IsInitialized)
        virtualMaps.Initialize();
    if (!virtualMaps.VirtualMapsProvider.IsInitialized)
        virtualMaps.VirtualMapsProvider.Initialize();


    // Make sure the properties are applied correctly
    bingMapLayerData.ApplyChanges();
    bingMapLayerData.Active = true;
    
}
```

## EPSG Search  

EPSG Code can be searched within the module. Click on the Map button to bring up a new window and type in the major location like Country, State, Provence etc. From the displayed list, double click on the desired row to automatically get the ESPG code from the selected item.

### EPSG Search Engine

```csharp
private static List<Epsg> SearchEpsgCodes(string keyword)
{
    var codes = new List<Epsg>();

    var uri = new Uri($"{EPSG_WEBISITE}{keyword}");
    var response = httpClient.GetAsync(uri).Result;
    if(response != null && response.IsSuccessStatusCode)
    {
        var rawData = response.Content.ReadAsStringAsync().Result;
        var regExPattern = @"<li.*\/epsg\/.*>(?'EpsgCode'.*)<\/a>(?'Description':.*)<\/li>";
        var regEx = new Regex(regExPattern);

        foreach(Match match in regEx.Matches(rawData))
        {
            var code = match.Groups["EpsgCode"].Value;
            var description = match.Groups["Description"].Value;
            codes.Add(new Epsg($"{code} {description}", code.Replace("EPSG:", "")));
        }
    }

    return codes;
}
```

## Open Project as StandAlone (rather than Unknown)

Since this project is related to background layers, we have to open up the project bit differently. Most of the time, we do not have to create a sub class of `WaterApplicationManager` but here we are creating one and modifying some properties as shown.

The default behavior is `LicensePlatformType.Unknown` which will result in some null reference exception when trying to work with background layers. So is the correct approach in this case.

```csharp
public class WaterAppManager : WaterApplicationManager
{
    protected override bool IsHeadless => false;

    protected override IDomainApplicationModel NewApplicationModel()
        => new WaterProductApplicationModel(LicensePlatformType.Standalone, "10.00.00.00", null);
}
```

## Other projects based on OpenFlows Water and/or WaterObjects.NET

* [Isolation Valve Adder](https://github.com/worthapenny/OpenFlows-Water--IsolationValveAdder)
* [Bing Background Adder](https://github.com/worthapenny/OpenFlows-Water--BingBackground)
* [Model Merger](https://github.com/worthapenny/OpenFlows-Water--ModelMerger)
* [Demand to CustomerMeter](https://github.com/worthapenny/OpenFlows-Water--DemandToCustomerMeter)

## Did you know?

Now, you can work with Bentley Water products with python as well. Check out:

* [Github pyofw](https://github.com/worthapenny/pyofw)
* [PyPI pyofw](https://pypi.org/project/pyofw/)

![pypi-image](https://github.com/worthapenny/OpenFlows-Water--ModelMerger/blob/main/images/pypi_pyofw.png "pyOFW module on pypi.org")
