Now that you've installed HC.RM.Common.Azure, there's a few things left to do:

Add a reference to "Application Insights for Web Applications"

```
PM> Install-Package Microsoft.ApplicationInsights.Web
```

==========================================================

#POST INSTALL CHANGES:

##Worker Role Project: 

Add your Application Insights key into `ApplicationInsights.config` as per the comments - this seems to enable the Application Insights Server Module to correctly send values to AI.

##Cloud Service:

Add a ConfigurationSettings `AppInsightsKey` and give it your Application Insights key.

Update the relevant WorkerRole in the ServiceDefinition.csdef to include the following Startup task:

```xml
<Startup>
    <Task commandLine="AppInsightsAgent\InstallAgent.bat" executionContext="elevated" taskType="simple">
    <Environment>
        <Variable name="ApplicationInsightsAgent.DownloadLink" value="http://go.microsoft.com/fwlink/?LinkID=522371" />
        <Variable name="RoleEnvironment.IsEmulated">
        <RoleInstanceValue xpath="/RoleEnvironment/Deployment/@emulated" />
        </Variable>
    </Environment>
    </Task>
</Startup>
```

Start enjoying Telemetry.