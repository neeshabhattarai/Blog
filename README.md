# ASP.NET Core Blog API Documentation

## Features
- User registration and authentication
- CRUD operations for blog posts
- Tag management
- Commenting system
- Search functionality

## Technology Stack
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Swagger for API documentation

## Architecture
The Blog API follows a microservices architecture, where each service is responsible for a specific part of the functionality.

## Setup Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/neeshabhattarai/Blog.git
   ```
2. Navigate to the project directory:
   ```bash
   cd Blog
   ```
3. Install the dependencies:
   ```bash
   dotnet restore
   ```
4. Set up the SQL Server database and update the connection string in `appsettings.json`.
5. Run the application:
   ```bash
   dotnet run
   ```

## API Endpoints
| Method | Endpoint                    | Description                  |
|--------|-----------------------------|------------------------------|
| GET    | /api/posts                  | Retrieve all posts           |
| POST   | /api/posts                  | Create a new post            |
| GET    | /api/posts/{id}             | Retrieve a specific post     |
| PUT    | /api/posts/{id}             | Update a specific post       |
| DELETE | /api/posts/{id}             | Delete a specific post       |

## Authentication
The API uses JWT tokens for user authentication. Users must register and log in to obtain a token, which should be included in the `Authorization` header of requests.

## Database Schema
The database consists of the following tables:
- Users
- Posts
- Comments
- Tags

## Known Issues
- Performance degradation with large datasets
- Occasional token expiration issues on user sessions.

## Contribution Guidelines
1. Fork the repository.
2. Create a new branch for your feature or bug fix:
   ```bash
   git checkout -b feature/MyNewFeature
   ```
3. Commit your changes:
   ```bash
   git commit -m 'Add some feature'
   ```
4. Push to the branch:
   ```bash
   git push origin feature/MyNewFeature
   ```
5. Open a pull request.

Happy coding!