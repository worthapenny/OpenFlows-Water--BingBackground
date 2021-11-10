# Apply Bing map background the easy way to WaterGEMS WaterCAD or WaterOPS

The application is written in C# using WaterObjects.NET API and OpenFlows Water API allowing an individual to easily apply the Bing map background with ease.

## Demo

[video]

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
