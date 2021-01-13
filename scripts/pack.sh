if [ $TRAVIS_PULL_REQUEST = false ]
then
    echo "Starting package publish"

    for dir in src/*/
    do
        dir=${dir%*/}
        echo Publishing nuget package $dir
        sh ./$dir/scripts/pack-pub.sh & wait
        echo ""
    done

    echo "Finished publishing package"
fi
if [ $TRAVIS_PULL_REQUEST != false ]
then
    echo "Not publishing packages"
fi