
pipeline {
    agent any
  
    stages {
        stage('Restore packages'){
           steps{
               sh 'dotnet restore ASP.Net_Core.sln'
            }
         }        
        stage('Clean'){
           steps{
               sh 'dotnet clean ASP.Net_Core.sln --configuration Release'
            }
         }
        stage('Build'){
           steps{
               sh 'dotnet build ASP.Net_Core.sln --configuration Release --no-restore'
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