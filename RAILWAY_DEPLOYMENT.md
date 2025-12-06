# 🚂 Railway.com Deployment Guide

This guide will walk you through deploying the DRB-Task Solution (API + SQL Server Database) on Railway.com.

---

## 📋 Prerequisites

1. **GitHub Account** - Your code must be pushed to a GitHub repository
2. **Railway Account** - Sign up at [railway.app](https://railway.app) (free tier available)
3. **Git installed** on your machine

---

## 🚀 Step-by-Step Deployment

### Step 1: Push Code to GitHub

If you haven't already, push your code to GitHub:

```bash
# Initialize git (if not already done)
git init

# Add all files
git add .

# Commit
git commit -m "Prepare for Railway deployment"

# Add remote (replace with your repo URL)
git remote add origin https://github.com/YOUR_USERNAME/DRB-Task-Solution.git

# Push to GitHub
git push -u origin main
```

---

### Step 2: Create Railway Account & Project

1. Go to [railway.app](https://railway.app)
2. Click **"Login"** → Sign in with **GitHub**
3. Click **"New Project"** on your dashboard

---

### Step 3: Deploy SQL Server Database

Railway supports SQL Server through Docker. Here's how to set it up:

#### Option A: Use Railway's SQL Server Docker Template (Recommended)

1. In your Railway project, click **"+ New"** → **"Docker Image"**
2. Enter the image: `mcr.microsoft.com/mssql/server:2022-latest`
3. Click **"Deploy"**
4. Go to the **"Variables"** tab and add:

| Variable | Value |
|----------|-------|
| `ACCEPT_EULA` | `Y` |
| `MSSQL_SA_PASSWORD` | `YourStrong@Password123!` (use a strong password) |
| `MSSQL_PID` | `Express` |

5. Go to **"Settings"** → **"Networking"** → Enable **"Private Networking"**
6. Note the internal hostname (something like `mssql.railway.internal`)

> ⚠️ **Important:** Use a strong password with uppercase, lowercase, numbers, and special characters!

#### Option B: Use Azure SQL Database (Cloud SQL Server)

If you prefer a managed SQL Server:

1. Go to [Azure Portal](https://portal.azure.com)
2. Create a new **Azure SQL Database**
3. Configure firewall rules to allow Railway's IPs
4. Get the connection string from Azure

---

### Step 4: Deploy the API from GitHub

1. In your Railway project, click **"+ New"** → **"GitHub Repo"**
2. Select your **DRB-Task-Solution** repository
3. Railway will automatically detect the `Dockerfile` and start building
4. Wait for the build to complete (2-5 minutes)

---

### Step 5: Configure Environment Variables for API

1. Click on your **API service** in Railway
2. Go to the **"Variables"** tab
3. Add the following variables:

| Variable | Value |
|----------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `PORT` | `8080` |
| `ConnectionStrings__DatabaseConnection` | See below |

#### Connection String Format:

**For Railway SQL Server (Option A - using private networking):**
```
Server=mssql.railway.internal;Database=DRB.InternDb;User Id=sa;Password=YourStrong@Password123!;TrustServerCertificate=True;Encrypt=False;
```

**For Azure SQL Database (Option B):**
```
Server=your-server.database.windows.net;Database=DRB.InternDb;User Id=your-user;Password=your-password;TrustServerCertificate=True;Encrypt=True;
```

> 💡 **Pro Tip:** Use Railway's variable references like `${{mssql.MSSQL_SA_PASSWORD}}` to reference the database password automatically.

---

### Step 6: Link Services (If Using Railway SQL Server)

To use private networking between services:

1. Click on your **API service**
2. Go to **"Variables"** tab
3. For the connection string, use Railway's reference syntax:

```
Server=${{mssql.RAILWAY_PRIVATE_DOMAIN}};Database=DRB.InternDb;User Id=sa;Password=${{mssql.MSSQL_SA_PASSWORD}};TrustServerCertificate=True;Encrypt=False;
```

This automatically injects the SQL Server hostname and password!

---

### Step 7: Generate Public Domain

1. Click on your **API service**
2. Go to **"Settings"** tab
3. Scroll to **"Networking"** section
4. Click **"Generate Domain"**
5. Railway will create a URL like: `https://drb-task-api-production.up.railway.app`

---

### Step 8: Verify Deployment

1. Open your generated domain in a browser
2. Navigate to `/swagger` to see the API documentation
3. Test your endpoints!

**Example URL:**
```
https://your-app-name.up.railway.app/swagger
```

---

## 📁 Files for Railway Deployment

| File | Purpose |
|------|---------|
| `Dockerfile` | Multi-stage build for .NET 8 API |
| `railway.json` | Railway deployment configuration |
| `docker-compose.yml` | Local development (not used by Railway) |

---

## 🔧 Troubleshooting

### Build Fails
- Check the **Deploy Logs** in Railway
- Ensure all project references are correct in `.csproj` files
- Verify `Dockerfile` syntax

### Database Connection Error
- Verify the SQL Server container is running (green status)
- Check if private networking is enabled
- Ensure password meets SQL Server complexity requirements
- Verify environment variable name: `ConnectionStrings__DatabaseConnection` (double underscore!)

### App Not Starting
- Check if `PORT` environment variable is set to `8080`
- View logs in Railway's **Deployments** tab
- Verify health check endpoint `/swagger` is accessible

### SQL Server Container Keeps Restarting
- Check if `ACCEPT_EULA=Y` is set
- Verify password complexity (min 8 chars, uppercase, lowercase, number, special char)
- Check Railway logs for specific error messages

---

## 🔄 Automatic Deployments

Railway automatically deploys when you push to your GitHub repository:

```bash
git add .
git commit -m "Update feature"
git push origin main
# Railway will auto-deploy! 🚀
```

---

## 💡 Environment Variable Reference

### API Service Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Runtime environment | `Production` |
| `PORT` | Port for the application | `8080` |
| `ConnectionStrings__DatabaseConnection` | SQL Server connection string | See above |

### SQL Server Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `ACCEPT_EULA` | Accept license | `Y` |
| `MSSQL_SA_PASSWORD` | SA password | `YourStrong@Password123!` |
| `MSSQL_PID` | SQL Server edition | `Express` |

---

## 💰 Railway Pricing

| Plan | Usage | Price |
|------|-------|-------|
| **Trial** | $5 credit (one-time) | Free |
| **Hobby** | $5/month credit | $5/month |
| **Pro** | Team features | $20/user/month |

> ⚠️ SQL Server on Railway consumes more resources. The Hobby plan should be sufficient for small projects.

---

## 🐳 Local Development with Docker

For local testing before deploying to Railway:

```bash
# Start both database and API locally
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down

# Stop and remove volumes (clean slate)
docker-compose down -v
```

Access locally at: `http://localhost:5000/swagger`

---

## 📚 Additional Resources

- [Railway Documentation](https://docs.railway.app/)
- [Railway Docker Images](https://docs.railway.app/guides/dockerfiles)
- [SQL Server on Docker](https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-docker-container-deployment)
- [.NET on Railway](https://docs.railway.app/guides/dotnet)

---

## 🎉 Success!

Once deployed, your API will be accessible at:
```
https://your-app-name.up.railway.app/swagger
```

Your SQL Server database and .NET API are now running on Railway! 🚂☁️

---

## 🔐 Security Best Practices

1. **Never commit real passwords** to your repository
2. Use **Railway's secret variables** for sensitive data
3. Enable **private networking** between services
4. Use **strong passwords** for SQL Server
5. Consider using **Azure Key Vault** for production secrets
