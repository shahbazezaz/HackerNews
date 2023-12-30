# HackerNews

RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n isspecified by the caller to the API.

The project is build in .Net 6.0

## Prerequisites

Before running this project, make sure you have the following prerequisites installed:

- .NET 6.0 SDK
- Visual Studio 22

## Packages
1. Microsoft.AspNet.WebApi.Client (6.0.0) : This package provides the necessary tools for consuming web APIs in a .NET application.
2. Microsoft.Extensions.Caching.Memory (8.0.0) : This package is used for caching the retrieved story IDs and the best n stories.
3. Newtonsoft.Json (13.0.3) : This package is used for JSON serialization and deserialization.


## Getting Started

To get started with this project, follow these steps:

1. Clone the repository:

   git clone https://github.com/shahbazezaz/HackerNews.git

2. Open the project in Visual Studio 2022.

3. Build the project to restore the NuGet packages and compile the code.

4. Run the application. This will launch the Swagger interface for interacting with the API.

# BestStoriesController

The BestStoriesController is a C# implementation that handles the retrieval of the best stories from the Hacker News API. It sorts the stories based on their score and returns the specified number of stories.

# Caching

The HackerNews API utilizes the MemoryCache feature to improve performance. Initially, the API retrieves and stores the IDs of the best stories in the cache. Subsequent requests for the best stories will be served from the cache, reducing the need for additional API calls.




