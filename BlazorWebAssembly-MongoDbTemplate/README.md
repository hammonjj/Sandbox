# Blazor Web Assembly MongoDb Template

This is a rough template for creating a Blazor Web Assembly app that connects to mongodb. Also includes other goodies such as authentication, localization, local storage, etc. Basically everything you need to immediately get to prototyping

Resources:
- https://medium.com/net-core/build-a-web-app-with-asp-net-core-and-mongodb-f9fcd61cb04f
- https://code-maze.com/unit-testing-aspnetcore-web-api/
- https://timdeschryver.dev/blog/how-to-test-your-csharp-web-api

## Things to Change When Forking
- Book.cs
- BookService.cs
- IBookService.cs
- BookController.cs
- BooksCatalogue.razor
- BookViewModel
- IBookViewModel
- Program.cs
  - builder.Services.AddHttpClient<IBookViewModel, BookViewModel>
	("MongoDbTemplate", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
- appsettings.json
  - "BookDb": "mongodb://localhost:27017"
- Startup.cs
  - services.AddScoped<BookService>();

## Localization

https://akmultilanguages.azurewebsites.net/
  - Use this for automatically translating more *.yml files

### How to Add a Localization
Upload english localization file toakmultilanguages.azurewebsites.net and select which localization you want
Place the file in the Resources folder and change the build action to "Embedded Resource"
Add localization to languages menu

Note: In order for pages to get the language changes, you must subscribe to the OnLanguageChange event in the MainLayout.razor page

https://github.com/aksoftware98/multilanguages
https://www.youtube.com/watch?v=Xz68c8GBYz4&feature=youtu.be

### Local Storage
https://www.nuget.org/packages/Blazored.LocalStorage/
https://wellsb.com/csharp/aspnet/blazor-write-to-localstorage/

## To Do
Add User Impersonation
  - Add role based authentication
