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
      sh  " cd .\\eShop.BackendApi\\ "
      sh "  dotnet build "  
      sh " dotnet run " 
        }
      }
    
    stage("build AdminApp") {
      steps {
      sh " cd .\\eShop.AdminApp\\ "    
      sh " dotnet build "  
      sh "  dotnet run "  
        }
      }

    stage("build WebApp") {
      steps {
      sh " cd .\\eShop.WebApp\\ "   
      sh " dotnet build "   
      sh " dotnet run "  
        
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