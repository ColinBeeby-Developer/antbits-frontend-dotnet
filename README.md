# nhsuk.base-application

[![Build Status](https://dev.azure.com/nhsuk/nhsuk.base-application/_apis/build/status/tomdoughty.nhsuk.base-application?branchName=main)](https://dev.azure.com/nhsuk/nhsuk.base-application/_build/latest?definitionId=658&branchName=main)

A .NET project which include some of the common things needed at NHS.UK.

[View the latest deployment of main branch](https://nhsuk-base-application-dev-uks.azurewebsites.net/service-name).

## Overview

[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io/#https://github.com/ColinBeeby-Developer/antbits-frontend-dotnet)

### ASP .NET Core web application
- ASP .NET Core MVC Views using .NET 3.1 Framework
- NHS.UK frontend library
- NHS.UK header and footer Nuget package ([on this branch](https://github.com/tomdoughty/nhsuk.base-application/tree/with-nhsuk-header-api))

### NUnit test project
Setup for NUnit unit tests.

### Azure pipeline
Automated CI pipeline to deploy pushes to `main` to nhsuk development environment.
- Checkout
- Nuget - restore packages including 
- Test - runs tests in NUnit test project
- Build - build web application
- Publish - publish web application
- Deploy - deploy to Azure web service


## ASP .NET Core web application
The application runs on Windows and Linux using either Visual Studio or CLI.

For dev the application will run on `https://localhost:5001/service-name` with the base path being configurable in `Startup.cs`

We always use base paths for applications so it is easier for infra to handle our application in higher environments and to ensure any assets are served from the correct application and not the domain root which would hit Wagtail.


### Build
Building the web application will automatically run `npm install && npm run build`. An incremental build is used so that these commands are only run if relevant files are modified, e.g. `main.scss` is modified.  
The commands can be forced to run by a Rebuild in Visual Studio or running `dotnet --no-incremental` if using the CLI.

The `npm install` command installs all NPM dependencies listed within the `package.json`.

The `npm run build` command runs the `gulp build` task.

### Gulp
The application contains 2 tasks in `gulpfile.js`.

The task `gulp build` will build CSS and JS assets and add them to `wwwroot/dist`. _These files are not commited to the repository._

The task `gulp` does the same as gulp build but adds a watch to recompile assets as they change in `wwwroot/src/**/*`.

### SCSS
A single SCSS file exists at `wwwroot/src/scss/main.scss`.

This file imports the required SCSS from NHS.UK frontend library.

Any custom SCSS can be added or imported into this file.

The Gulp tasks compile this file into CSS and minify it for production.

The resulting JavaScript is saved in `wwwroot/dist/main.css`. _This file is not commited to the repository_.

### JavaScript
A single JavaScript file exists at `wwwroot/src/js/main.js`.

This file imports the required JavaScript from NHS.UK frontend library.

Any custom ES2015 JavaScript can be added or imported into this file.

The Gulp tasks transpile this file into ES5 JavaScript using Babel and minify it for production. The resulting JavaScript is saved in `wwwroot/dist/main.js`. _This file is not commited to the repository_.

### NHS.UK header and footer Nuget package
The header and footer can be dynamically built by the `nhsuk.header-and-footer-client` Nuget package. If this is a feature you would like then an example implementation is [on this branch](https://github.com/tomdoughty/nhsuk.base-application/tree/with-nhsuk-header-api).

This takes a bit of configuration hence it not being in the main branch. NHS.UK header and footer Nuget package is available by [connecting to the Azure feed](https://dev.azure.com/nhsuk/nhsuk.header-footer-api-client/_packaging?_a=connect&feed=nhsuk.header.footer.api.client%40Release).

### Adobe analytics
Adobe analytics script is loaded in based on `AdobeAnalyticsScriptUrl` set in `appsettings.json`.

Adobe analytics `digitalData` object is built dynamically from application URL.

### Cookie banner
NHS.UK cookier banner is loaded in based on `CookieScriptUrl` set in `appsettings.json`.

### Docker
Developing with Docker on .NET applications is dreadful unfortunately. It is very very slow at rebuilds and you would be better off using .NET CLI if you wish to avoid Visual Studio, that is what the Dockerfile is doing in the background for you anyway.

Some features will not work with docker-compose as it is not running HTTPS out of the box. This requires some additional configuration on your own machine. Details can be found here https://thegreenerman.medium.com/set-up-https-on-local-with-net-core-and-docker-7a41f030fc76

If you want to use the NHS.UK header and footer API you need to generate a Personal Access Token in Azure Devops and pass this to `docker-compose`.

The main thing we use Docker for on these projects is to build an image, store it in Azure registry and deploy to Azure Kubernates Service.

## Examples

### Multi step form
https://nhsuk-base-application-dev-uks.azurewebsites.net/example-form

A lot of new services at NHS.UK are transactional. This form will store input in `TempData` across pages which can be used to display a summary page to check answers. This form also handles validation with `IValidateObject` as we have rewritten validation across many apps now. This is implemented with fully a accessible error summary, and error messages.

### Results from API
https://nhsuk-base-application-dev-uks.azurewebsites.net/example-async?org=nhsuk

This page gets an organisation's repositories from GitHub API and displays them on the page.

## Licence

The codebase is released under the MIT Licence, unless stated otherwise. This covers both the codebase and any sample code in the documentation. The documentation is © Crown copyright and available under the terms of the Open Government 3.0 licence.
