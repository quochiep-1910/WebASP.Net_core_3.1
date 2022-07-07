pipeline {

  agent none
  stages {
    stage("Test") {
    
      steps {
        dotnet build
      }
    }

    stage("build BackEndApi") {
      steps {
        cd .\eShop.BackendApi\
        dotnet build
        dotnet run
        }
      }
    
    stage("build AdminApp") {
      steps {
        cd .\eShop.AdminApp\
        dotnet build
        dotnet run
        }
      }

    stage("build WebApp") {
      steps {
        cd .\eShop.WebApp\
        dotnet build
        dotnet run
        
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