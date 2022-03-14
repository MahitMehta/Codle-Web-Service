# About

Function to Generate Random Codle Puzzle.

# Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

# If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

# Execute unit tests
```
    cd "GenerateCodle/test/GenerateCodle.Tests"
    dotnet test
```

# Deploy function to AWS Lambda
```
    cd "GenerateCodle/src/GenerateCodle"
    dotnet lambda deploy-function
```
