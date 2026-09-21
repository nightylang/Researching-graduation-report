## 🌐 Cross-Platform Production Architecture

   [ Mobile Phone / Web Client ] 
             │
             ▼ (Port 80 / HTTP)
   [ Ubuntu Server Node ] ───> Host: Apache or Nginx Web Reverse Proxy
             │                 Runs: ASP.NET Core Runtime Environment
             │
             ▼ (Port 1433 / TCP Cross-Server Network Handshake)
    [ Windows Server Node ] ───> Runs: Microsoft SQL Server Engine

# Database Connectivity Configuration Guide

## 🛠️ Step 1: Open the Port Pipeline on Windows Server

Before touch-testing Ubuntu, you must ensure your Windows Server is configured to accept incoming database connections from an external Linux machine.

### Prerequisites
* Administrative access to the Windows Server hosting the SQL instance.
* Default SQL Server port configuration (**TCP 1433**).

### Execution Steps

1. **Launch Firewall Manager**
   On your Windows Server machine, open **Windows Defender Firewall with Advanced Security**.

2. **Initiate New Rule Wizard**
   Click **Inbound Rules** in the left panel, then select **New Rule...** in the right panel.

3. **Specify Rule Type**
   Select **Port** and click **Next**.

4. **Define Protocol and Ports**
   Choose **TCP** and enter **Specific local ports**: `1433`, then click **Next**.

5. **Configure Action Profile**
   Select **Allow the connection** and click **Next**.

6. **Apply Network Profile Scope**
   Check **Domain**, **Private**, and **Public**, then click **Next**.

7. **Finalize and Deploy**
   Name the rule `Allow_SQL_From_Ubuntu` and click **Finish**.

## 📦 Step 2: Install .NET Runtime Engine on Ubuntu Server

Ubuntu cannot compile or execute C# source binaries natively. You must load Microsoft’s official Linux execution runtime environment before deploying your applications.

### Prerequisites
* An active SSH connection or local terminal access to your Ubuntu Server.
* An account with `sudo` administrative privileges.

### Execution Steps

1. **Refresh System Repository Indices**
   Connect to your Ubuntu Server terminal and update your local package indexes to ensure you pull the latest software definitions:
   ```bash
   sudo apt-get update
   ```

2. **Install the .NET Runtime Package**
   Install the official Microsoft .NET Runtime package matching your project targets (e.g., .NET 8.0 or .NET 9.0). For standard systems, run:
   ```bash
   sudo apt-get install -y dotnet-runtime-8.0
   ```

3. **Verify Host Environment Setup**
   Confirm that the runtime stack is successfully provisioned and check its environment telemetry by executing:
   ```bash
   dotnet --info
   ```

## 🚀 Step 3: Publish & Transport the App Build to Ubuntu

You must pre-compile your project files on your development laptop before transferring them to the Linux target directory layout.

### Prerequisites
* **Visual Studio IDE** installed on your development machine.
* An SFTP client application (e.g., **FileZilla** or **WinSCP**).
* `write` permissions assigned to the target deployment path on your Ubuntu instance.

### Execution Steps

1. **Initiate the Compilation Pipeline**
   Open your code workspace in Visual Studio. Right-click your core project name in the Solution Explorer and select **Publish**.

2. **Configure Target Profile**
   Choose **Folder** as the target configuration destination, then set your target path to a local folder on your development laptop.

3. **Tune Advanced Profile Settings**
   Open the **Advanced Profile Settings** configuration panel and set these explicit deployment criteria parameters:
   * **Deployment Mode:** `Framework-Dependent`
   * **Target Runtime:** `Portable` (or `Linux-x64`)

4. **Execute Local Compilation**
   Click the **Publish** button to compile the codebase down into raw deployment assets.

5. **Transport Assets to Target Host**
   Use your SFTP client tool to connect to your Ubuntu Server. Upload the compiled folder contents directly into the following clean Linux directory path:
   ```text
   /var/www/HR_Workspace_System/
   ```

## 🔀 Step 4: Configure the Connection Matrix (appsettings.json)

You must update your application settings inside the Ubuntu terminal to target your external Windows Server IP address instead of using `localhost`.

### Prerequisites
* The **Local IP Address** of your Windows Server machine.
* Standard SQL Server database authentication credentials (e.g., `sa` user login).

### Execution Steps

1. **Navigate to the App Deployment Root**
   On your Ubuntu terminal, switch directories to access your live production files:
   ```bash
   cd /var/www/HR_Workspace_System/
   ```

2. **Access the Application Settings File**
   Open the application configuration document using the `nano` terminal text editor interface:
   ```bash
   sudo nano appsettings.json
   ```

3. **Modify the Connection String Block**
   Locate your data source definitions and update your connection string matrix block. Ensure the `Server` parameter points directly to your target Windows Server IP address:
   ```json
   "ConnectionStrings": {
     "WorkspaceEnterpriseDB": "Server=YOUR_WINDOWS_SERVER_IP_HERE;Database=HR_Workspace_DB;User Id=sa;Password=YourSecurePassword;TrustServerCertificate=True;MultipleActiveResultSets=True;"
   }
   ```

4. **Persist Modifications and Exit**
   Press `Ctrl + O` then hit `Enter` to commit your changes to disk. Press `Ctrl + X` to exit the text editor wrapper.

## 🛡️ Step 5: Route Web Traffic using an Apache Reverse Proxy

Since ASP.NET Core apps run internally on private port `5000` by default, you must use a standard Linux web server to route incoming traffic from public web port `80` down to your C# engine.

### Prerequisites
* Port `80` open on your Ubuntu Server's firewall.
* Complete administrative (`sudo`) shell privileges.

### Execution Steps

1. **Install the Apache Web Server Core**
   Provision the base Apache package deployment onto your local Ubuntu architecture:
   ```bash
   sudo apt-get install -y apache2
   ```

2. **Activate the Network Proxy Subsystem Modules**
   Enable the explicit tracking and routing runtime modules so Apache can cleanly tunnel and map backend network traffic:
   ```bash
   sudo a2enmod proxy proxy_http proxy_html headers
   ```

3. **Generate a Dedicated VirtualHost Definition File**
   Provision an isolated site configuration matrix for your project layout using the terminal text editor:
   ```bash
   sudo nano /etc/apache2/sites-available/hr_workspace.conf
   ```

4. **Map the Routing Rules Block**
   Paste the following routing guidelines straight into the configuration file to direct root web requests downward onto your internal application layer:
   ```apache
   <VirtualHost *:80>
       ProxyPreserveHost On
       ProxyPass / http://127.0.0
       ProxyPassReverse / http://127.0.0
       ErrorLog ${APACHE_LOG_DIR}/hr_workspace-error.log
       CustomLog ${APACHE_LOG_DIR}/hr_workspace-access.log combined
   </VirtualHost>
   ```
   *Press `Ctrl + O` to save and `Ctrl + X` to exit.*

5. **Deploy the Site Logic and Cycle the Daemon Engine**
   Activate the workspace profile, disable the default Apache placeholder page index, and restart the core platform services to apply your updates:
   ```bash
   sudo a2ensite hr_workspace.conf
   sudo a2dissite 000-default.conf
   sudo systemctl restart apache2
   ```

## 🔄 Step 6: Create an Automated Background System Daemon

To prevent your web portal from crashing the moment you close your terminal window, you must configure a system daemon worker to keep the C# app running constantly in the background.

### Prerequisites
* The location path of your compiled binary assembly (`HR_Workspace_System.dll`).
* Correct file access permissions granted to the `www-data` user system identity group.

### Execution Steps

1. **Initialize the Service Blueprint File**
   Open an empty configuration context file inside the system daemon pool folder infrastructure:
   ```bash
   sudo nano /etc/systemd/system/kestrel-hr.service
   ```

2. **Define the Engine Process Directive**
   Paste the following parameters to instruct the OS architecture how to manage, track, and sustain the long-running application stack:
   ```ini
   [Unit]
   Description=ASP.NET Core Workspace Application Running on Ubuntu

   [Service]
   WorkingDirectory=/var/www/HR_Workspace_System
   ExecStart=/usr/bin/dotnet /var/www/HR_Workspace_System/HR_Workspace_System.dll
   Restart=always
   RestartSec=10
   KillSignal=SIGINT
   SyslogIdentifier=dotnet-workspace-app
   User=www-data
   Environment=ASPNETCORE_ENVIRONMENT=Production

   [Install]
   WantedBy=multi-user.target
   ```
   *Press `Ctrl + O` to save and `Ctrl + X` to exit.*

3. **Register and Boot the Automation Architecture**
   Reload the systemd daemon to pick up the new file, enable the program to start automatically on system reboot, and kick off the workflow immediately:
   ```bash
   sudo systemctl daemon-reload
   sudo systemctl enable kestrel-hr.service
   sudo systemctl start kestrel-hr.service
   ```

---

## 🎯 Step 7: Live Execution Check-In Demo

Open any browser connected to your local office routing network and type your Ubuntu Server's IP address directly into the address bar:
```text
http://192.168.10.50
```

Your **ASP.NET Core MVC Public Check-In Portal** will load instantly on the client screen! The deployment will seamlessly capture incoming device fingerprints and telemetry tokens, securely transmitting the parameterized payloads across the open network pipeline straight into your **Windows SQL Server database ledger** with zero data latency.


# The end

