# Hello World Application

Helloworld application which stores username and date of birth in the database which can be deployed in Kubernetes 

## Source Code

The application is written using C# WebAPI , currently supports two endpoints. The application stores its data in Lite DB. LiteDB is a small, fast and lightweight .NET NoSQL embedded database.

1. Create/update user details -Date of Birth 

    ```PUT /hello/<username> { “dateOfBirth”: “YYYY-MM-DD” }```

2. Returns hello birthday for a given user

     ```GET /hello/<username>```

## Design

Design contains 3 components

    1. CI / CD for Hello World
    2. HA for Hello World in EKS
    3. Hello World Namespace design 

## Deployment

The deployment can be made locally using docker file or a docker compose file.

It has a `deployment.yaml` file which can be deployed in kubernetes

I have created a `Jenkinsfile` to deploy helloworld application.However this is just pseudocode
