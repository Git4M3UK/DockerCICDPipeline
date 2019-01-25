# BUILD STAGE
	FROM microsoft/dotnet:2.2-sdk as build

	WORKDIR /api

# RESTORE

	# Restore project
	COPY api/api.csproj ./api/
	RUN dotnet restore api/api.csproj

	# Restore test - This comes after Api because it has a dependency
	COPY tests/tests.csproj ./tests/
	RUN dotnet restore tests/tests.csproj

	# List files copied in
	RUN ls -alR 


# COPY SOURCE
	COPY . .

# TEST - Build will stop here is tests fail. then ENV variable will trigger better reporting mode in XUnit runner
	ENV TEAMCITY_PROJECT_NAME=fake

	RUN dotnet --verbosity=normal test tests/tests.csproj

# PUBLISH
	RUN dotnet publish api/api.csproj -c Release -o /publish


# RUNTIME STAGE
	FROM  microsoft/dotnet:2.2-aspnetcore-runtime as runtime

	# Set new working directory
	WORKDIR /publish

	# Copy publish files from build-env to runtime-env
	COPY --from=build /publish .

	# Set starting point
	ENTRYPOINT ["dotnet", "api.dll"]
