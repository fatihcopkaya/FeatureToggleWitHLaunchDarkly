# Feature Toggle with LaunchDarkly and Consul

This project demonstrates the implementation of feature toggling using LaunchDarkly and Consul. The feature flags are managed in LaunchDarkly, and the configuration (SDK key and flag name) is stored and retrieved from Consul. The Consul service is run using Docker.

## Project Structure

- **Services**
  - `ConsulHostedService`: A hosted service to periodically fetch the SDK key and flag name from Consul.
  - `FeatureToggleService`: A service to check if a feature is enabled for a given user and role.
  - `LdClientProvider`: A provider to manage the LaunchDarkly client instance.

- **Interfaces**
  - `IFeatureToggleService`: Interface for the feature toggle service.
  - `ILdClientProvider`: Interface for the LaunchDarkly client provider.

- **Controllers**
  - `WeatherForecastController`: A sample controller to demonstrate feature toggling.

- **IOC**
  - `IOC`: Extension method to register services with dependency injection.

## Setup

### Prerequisites

- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Consul](https://www.consul.io/downloads)
- [LaunchDarkly](https://launchdarkly.com/)

### Consul Setup

1. Start Consul in Docker:

    ```bash
    docker run -d --name=dev-consul -e CONSUL_BIND_INTERFACE=eth0 -p 8500:8500 consul
    ```

2. Add the necessary keys to Consul:

    ```bash
    curl --request PUT --data "your-launchdarkly-sdk-key" http://localhost:8500/v1/kv/config/launchdarkly/sdkKey
    curl --request PUT --data "your-flag-name" http://localhost:8500/v1/kv/config/launchdarkly/flagName
    ```

    Replace `"your-launchdarkly-sdk-key"` and `"your-flag-name"` with your actual LaunchDarkly SDK key and feature flag name.

### LaunchDarkly Setup

1. **Create a Feature Flag:**
    - Log in to your LaunchDarkly account.
    - Navigate to the **Flags** section.
    - Click on **Create a flag**.
    - Enter a name for your flag (this will be the flag name you add to Consul).
    - Set the flag key to match the flag name.
    - Choose a flag variation type (boolean, multivariate, etc.), typically boolean.
    - Save the flag.

2. **Create a Segment:**
    - Navigate to the **Segments** section.
    - Click on **Create a segment**.
    - Enter a name for your segment (e.g., "StandardUsers").
    - Define the segment rules, such as:
        - Add rule: Match users with `role` attribute equal to "Standard".
    - Save the segment.

3. **Add Segment to Feature Flag:**
    - Go back to the **Flags** section and select the flag you created.
    - Go to the **Targeting** tab.
    - Under **Add individual targets**, select **Add segment rule**.
    - Choose the segment you created (e.g., "StandardUsers").
    - Set the desired flag variation for this segment (e.g., true for enabled).
    - Save the changes.

### Project Setup

1. Clone the repository:

    ```bash
    git clone https://github.com/your-repo/feature-toggle-launchdarkly.git
    cd feature-toggle-launchdarkly
    ```

2. Restore dependencies:

    ```bash
    dotnet restore
    ```

3. Update `appsettings.json` or environment variables with your Consul address:

    ```json
    {
      "Consul": {
        "Address": "http://localhost:8500"
      }
    }
    ```

4. Run the application:

    ```bash
    dotnet run
    ```

## Usage

The application provides a simple endpoint to demonstrate feature toggling.

### Endpoint

- `GET /WeatherForecast`

  The endpoint will return weather forecasts only if the feature flag is enabled for the user with ID "1" and role "Standart".

## Important Files

- `ConsulHostedService.cs`: Manages the periodic fetching of configuration from Consul.
- `FeatureToggleService.cs`: Checks the status of the feature flag.
- `LdClientProvider.cs`: Provides the LaunchDarkly client instance.
- `WeatherForecastController.cs`: Example controller demonstrating the use of feature toggling.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.

## License

This project is licensed under the MIT License.

## Contact

For any questions, please contact [your-email@example.com].
