echo "Starting package publish"

for dir in src/*/
do
    dir=${dir%*/}
    echo Publishing nuget package $dir
    sh ./$dir/scripts/pack-pub.sh & wait
    echo ""
done



echo "Finished publishing package"