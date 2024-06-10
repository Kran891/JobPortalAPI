
# Job Portal API 

The **JobPortalAPI**  is a C# project aimed at creating a job portal API. This API facilitates job listings, user applications, and overall management of job-related data. The repository contains various components such as controllers, entities, migrations, models, repositories, and configuration files necessary to run and develop the application.

The setup involves configuring the database, installing dependencies, and running the server. Key functionalities include creating, updating, retrieving, and deleting job postings, as well as managing user applications.


## Tech Stack

**Server:** .Net Core, JWT, Jenkins, SonarQube,
Git, GitHub, Docker, DockerHub 

**Deployment:** Azure, MSSQL express 

## FrontEnd Reference

For FrontEnd please refer this repository:
https://github.com/Kran891/jobportalreact.git
## Installation



```bash
  add-migration intial
  update database --verbose
```
    
## Deployment

Once create a image in Docker push the image to DockerHub use the image name in DockerHub in Azure to create an instance and before that create a database in SQL Express by creating the instance of the SQL Server in Azure.

```bash
  add-migration intial
  update database --verbose
```


## Run Locally

Clone the project

```bash
  git clone https://github.com/Kran891/JobPortalAPI.git
```

Go to the project directory

```bash
  cd JobPortalAPI
```

Install dependencies

```bash
  add-migration intial
  update database --verbose
```

Build the application

```bash
  dotnet build
```

Run the application

```bash
  dotnet run
```


## Authors

- Github Profile
     - [@Kranthi Kumar Gavireddy](https://www.github.com/kran891)
-  Contact details:
    - Kranthi Kumar GaviReddy
      - 📞 +918688348532
      - 📧 kranthi.gavireddy.code@gmail.com

