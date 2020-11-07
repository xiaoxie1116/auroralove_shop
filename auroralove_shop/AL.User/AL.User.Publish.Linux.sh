git pull;
rm -rf .PublishFiles;
dotnet build;
dotnet publish -o /home/auroralove_shop/auroralove_shop/AL.User/AL.User.WebApi/bin/Debug/netcoreapp3.1;
cp -r /home/auroralove_shop/auroralove_shop/AL.User/AL.User.WebApi/bin/Debug/netcoreapp3.1 .PublishFiles;
echo "Successfully!!!! ^ please see the file .PublishFiles";