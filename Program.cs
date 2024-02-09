var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

//create data
Dictionary<int, string> countries = new Dictionary<int, string>()
{
    {1,"United States" },
    {2,"Canada" },
    {3,"Thailand" },
    {4,"Myanmar" },
};

//endpoints
app.UseEndpoints(endpoints =>
{
    //check request is "/"
    endpoints.MapGet("/countries", async (context) =>
    {
        foreach(KeyValuePair<int, string> country in countries)
        {
            //write all the countries
            await context.Response.WriteAsync($"{country.Key}, {country.Value}\n");
        }

    });

    //when request path is "countries/{countryID}"
    endpoints.MapGet("/countries/{countryID:int:range(1,100)}", async (context) =>
    {
        //check if countryID in request
        if (context.Request.RouteValues.ContainsKey("countryID") == false)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("The CountryID should be between 1 and 1000");
        }
        //read CountryID from routeValues

        int countryID = Convert.ToInt32(context.Request.RouteValues["countryID"]);

        //if countryID exist in countries dictionary
        if (countries.ContainsKey(countryID))
        {
            string countryName = countries[countryID];
            //return country Name
            await context.Response.WriteAsync($"{countryName}");
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Country doesn't exist");
        }


    });
    endpoints.MapGet("/countries/{countryID:int:min(101)}", async (context) =>
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("The countryID should be between 1 and 100");
    });
    
});

app.Run( async context =>
{
    await context.Response.WriteAsync("No Response");
});

app.Run();

