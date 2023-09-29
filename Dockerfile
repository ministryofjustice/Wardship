# Pull the Windows Server IIS base image
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

# Create a directory for your application
WORKDIR C:/app

# Copy your WebApp.zip file into the container
COPY WebApp.zip .

# Unzip the WebApp.zip file
RUN powershell -Command " \
    Expand-Archive -Path C:\app\WebApp.zip -DestinationPath C:\temp_extracted; \
    xcopy C:\temp_extracted\Content\D_C\a\wardship\wardship\Wardship\obj\Release\Package\PackageTmp\* C:\inetpub\wwwroot /E /I; \
    Remove-Item -Path C:\app\WebApp.zip -Force; \
    Remove-Item -Recurse -Force C:\temp_extracted \
    "

# Download ServiceMonitor
RUN powershell -Command " \
    Invoke-WebRequest -Uri 'https://dotnetbinaries.blob.core.windows.net/servicemonitor/2.0.1.6/ServiceMonitor.exe' -OutFile 'C:\ServiceMonitor.exe' \
    "

# Expose the IIS port
EXPOSE 80

# Start ServiceMonitor to manage the W3SVC service
ENTRYPOINT ["C:\\ServiceMonitor.exe", "W3SVC"]
