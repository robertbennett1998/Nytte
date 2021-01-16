cd src/Nytte.Testing

if [ $TRAVIS_BRANCH = "staging" ]
then
    echo Updating Nytte.Testing alpha package $TRAVIS_BRANCH
    dotnet pack -c Release /p:PackageVersion=0.4.$TRAVIS_BUILD_NUMBER-alpha -o .
    dotnet nuget push *.nupkg -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json
fi


