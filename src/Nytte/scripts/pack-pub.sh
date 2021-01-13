cd src/Nytte

if [ $TRAVIS_BRANCH = "staging" ]
then
    echo Updating Nytte alpha package $TRAVIS_BRANCH
    dotnet pack -c Release /p:PackageVersion:0.1.$TRAVIS_BUILD_NUMBER-alpha -o .
fi

dotnet nuget push *.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json