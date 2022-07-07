
pipeline {
    agent any
  
    stages {
        stage('Restore packages'){
           steps{
              dotnet restore ASP.Net_Core.sln
            }
         }        
        stage('Clean'){
           steps{
              dotnet clean ASP.Net_Core.sln --configuration Release
            }
         }
        stage('Build'){
           steps{
               dotnet build ASP.Net_Core.sln --configuration Release --no-restore
            }
         }
    }


  post {
    success {
      echo "SUCCESSFUL"
    }
    failure {
      echo "FAILED"
    }
  }
}