# BUILD STAGE 

	# Set build image
	FROM microsoft/dotnet:2.2-sdk as build

	# Set working directory
	WORKDIR /api

	# Copy over project file
	COPY *.csproj .

	# Restore dependencies
	RUN dotnet restore

	# Copy in all source files except files lists in .dockerignore
	COPY . .

	# Publish the files to the /publish directory
	RUN dotnet publish -c Release -o /publish

# RUNTIME STAGE

	# Set runtime image
	FROM  microsoft/dotnet:2.2-aspnetcore-runtime as runtime

	# Set new working directory
	WORKDIR /publish

	# Copy publish files from build-env to runtime-env
	COPY --from=build /publish .

	# Set starting point
	ENTRYPOINT ["dotnet", "api.dll"]