# Shippable.com sample project 

The problem requires to create a repository on GitHub and write a program in any programming language that will do the following:

Input: User can input a link to any public GitHub repository

Output: Your UI should display a table with the following information

- Total number of open issues
- Number of open issues that were opened in the last 24 hours
- Number of open issues that were opened more than 24 hours ago but less than 7 days ago
- Number of open issues that were opened more than 7 days ago


### Deploy your application to Heroku or similar platform.
- Deployment happnes automatically when any change happens to the master branch. The enitire solution is deployed to the Azure App Service

### See the MVC site here: 
- <a href="http://octokitgithubclient.azurewebsites.net/Result">http://octokitgithubclient.azurewebsites.net/</a>


	- Site is very simple MVC 5 ASP.NET site with two pages [Home and Result]. 
      - Home takes in the url, hwere it undergoes some simple validation (needs to be hardened)
      - Result uses the OctoKit .NET GitHub Client Library and extracts the issues from the passed-in repository. 

      - although the result page returns just the **amount** of issues per filtered standard as requested, the **/Classes/IssueHelper Class** collects all the information of each issue, including date, author, comment, etc and returns it to the view for eventual parsing. 
    - 
