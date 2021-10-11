Example Project with Boring Business Case
====

This repository contains example project that demonstrates TDD, BDD, DevOps and some other guidelines that I like to follow. The aim is to implement a business case that is simple enough to focus on the technical aspects, but still have some interesting features. The boring business case is to give users movie recommendations based on their genre preference.

Technical stack
===

The project contains ASP.NET REST API and simple React frontend which is deployed to Kubernetes using helm. The repository is hosted in GitHub and there is workflow that triggers on every push to verify the validity of each push.

- C# on .NET Core 3.1
- JavaScript with React, Redux and Redux-Saga
- HTML with Tailwind
- Kubernetes deloyed using helm


FAQ
===

- What kind of architecture did you use to achieve this Boring Business Case?
  - Since movies are something that can be watched and are categorized to genres, it is natural to have a REST API that talks about movies instead of genres directly. Users can watch movies and they can like or dislike them. And since each movie has one or multiple genres, we can infer the genres that the user likes this way. Then we can use some external movie database to search movies that share the genres with the movies that the user has liked already.

- How did you design the application for testability?
  - I have been following these steps to achieve testability:
    1. Have a concrete and exact specification of use cases. Please see the use cases below in Gherkin.
    2. Do the minimum to only implement the required use cases, and nothing else.
    3. Use dependency injection and loose coupling.
    4. Prefer interfaces.
    5. Do not duplicate code.
    6. Do not use hidden states.
    7. Avoid using global data.
    8. Have tests in multiple levels to make debugging easier and let you to pin-point the problems more easily. In this application, there are unit and integration tests. Unit tests test the code in method level and integration tests test the code in API level.
    9. Make your application easy to run. I like Kubernetes and skaffold since that is easy for developers to use and local development environment is close to production as possible. Basically, you can get the application up and running with just one command.
    10. Have a documentation about the design, implementation and something for users that they know how to use your application. You are reading that now!

- How would you take persistence into account in the design?
  - I have already implemented `IUnitOfWork` interface that contains all the data models used by the application. The unit of work instance gets disposed after every HTTP request which means that in persistence mode, it could make a transaction of all changed data and commit that into database. It is easy to implement the interface and make it to use Entity Framework database context, similarly to `InMemoryUnitOfWork` and `InMemoryDataContext`. Then the implementation can be injected in the place of `InMemoryUnitOfWork` and that's it.

- If you later decided to migrate to a microservices architecture, what kind of patterns would you use for this?
  - I think the best way would be to separate external services, application APIs (even endpoints!) and possible database to their own microservices. This way we could scale each service individually based on the need and maybe let other teams to have responsibility of each service. There are multiple ways to actually implement it, but I personally prefer Kubernetes a lot since it introduces an ecosystem that can run almost everywhere. In a cloud environment, there are built-in PaaS services that let you to ship the code and run it, such as Azure Functions and Lambdas in AWS. However, I would be very cautious about these since their pricing can surprise if there are service traffic peaks. In addition, their run environments can change without you noticing. Docker solves these problems since it packs the whole run environment with the application. Fortunately, there are also PaaS services (like Azure AKS and Amazon EKS) that can provision virtual machines automatically which are capable of running Docker on top of Kubernetes. This way, you know exactly that your application will always work and know exactly how much it costs.


Use cases
===

Here is a list of the use cases that the application consist of. Each use case has a matching BDD style integration test to verify REST API and use cases are implemented in `frontend/scenarios`.

**Scenario: Generate access token**\
**When** user requests an access token\
**Then** generate a JWT token

**Scenario: Like a movie**\
**Given** user has an access token\
**When** user likes a movie\
**Then** add the liked movie to the user preference

**Scenario: Dislike a movie**\
**Given** user has an access token\
  **And** user has liked the movie\
**When** user dislikes a movie\
**Then** remove the liked movie from the user preference

**Scenario: Get movie recommendations for user based on genre preference**\
**Given** user has an access token\
**When** user requests more movie recommendations\
**Then** extract unique genres from user's liked movies\
 **And** query next chunk of movies from a movie library\
 **And** list a movie if the movie's genres contain any of the extracted genres\
 **And** filter out the movies that user has liked already

**Scenario: Get paginated movies from movie library**\
**Given** user has an access token\
**When** user scrolls down the movie list\
**Then** query next chunk of movie results from movie library

**Scenario: Get user preference movies**\
**Given** user has liked some movies\
**When** user queries which movies are their preference\
**Then** create a subset of given movies to contain only ones that user has liked


Development & Usage
===

The best way to run the application is to use `skaffold` -- an awesome tool that can manage the whole application ecosystem. You can simply start continuous development that automatically builds Docker images and deploys them to local Kubernetes cluster:

```bash
skaffold dev
```

The most simple way for setup a local Kubernetes is to use Docker for Windows (wsl) or Docker for Mac that has Kubernetes enabled in Docker settings. Then you will also need an ingress controller deployed to your local Kubernetes:

```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.0.3/deploy/static/provider/cloud/deploy.yaml
```

This will deploy an ingress controller so that you can access applications that are running in Kubernetes which is accessable through `http://localhost`. That's it, now you are able to deploy any application ecosystem locally which match the ecosystem running in cloud!


Alternatively, you can just use Visual Studio 2019 with .NET Core and ASP.NET toolkit for REST API and Visual Studio Code for frontend. Open a solution file and then you can start the REST API by pressing F5. However, make sure that the target is `tech.haamu.Movie` which uses Kestrel standalone web server and not IIS. You can run unit and integration tests with a Test Explorer. You can start frontend as you would to start any `npm` based project: `npm install && npm run dev`. However, please keep in mind that frontend assumes that API is used through Kubernetes. Please change `getApiUrl()` accordingly.
