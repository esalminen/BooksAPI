# BooksAPI (.NET 6.0)

## Getting Started

Tools required to build and run the project:
- [.NET SDK](https://dotnet.microsoft.com/download) - includes the .NET CLI, runtime, and libraries
- (Optional) [Postman](https://www.postman.com/downloads/) for testing the API

### Installation

1. Clone or download the repository from
   ```sh
   git clone https://github.com/esalminen/BooksAPI.git
   ```

2. Start the API by running ``` dotnet run``` from the command 
   line in the project root folder (where the BooksAPI.csproj file is located).
   You should then see the message ```Now listening on: http://localhost:9000```

3. (Optional) Import the Postman collection from the ```BooksAPI.postman_collection.json``` file
   in the project root folder. This will createa a new collection with all the API tests.
 
## Usage

API documentation is available at ```http://localhost:9000/swagger/index.html``` 
when the API is running. API can be also tested from the Swagger UI.

(Optional) Postman test sequence can be run with Collection runner. Scripts
handle database initialization and test sequence execution.


Esa Salminen - [LinkedIn](https://www.linkedin.com/in/esa-salminen-9398421ba/) - esa_salminen@hotmail.com
