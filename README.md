# SamuraiApp API README

> Version 1.0 – generated automatically from Swagger (OpenAPI 3.0.4) on **27 May 2025**
> Slight modifications made by the author.

## 📌 Project purpose

* RESTful service that manages **Samurai**, their **Dojo**, and **Kenjutsu** styles
* Built with ASP.NET Core + Entity Framework + JWT authentication
* Deployed to Microsoft Azure (App Service)

## 🌐 Base URL

```
https://<your‑app>.azurewebsites.net
```

Replace *\<your‑app>* with the hostname of your deployment.

| Environment | Swagger UI            | Raw OpenAPI (JSON)         |
| ----------- | --------------------- | -------------------------- |
| Production  | `/swagger/index.html` | `/swagger/v1/swagger.json` |

## 🔐 Authentication flow

1. **Register** → `POST /auth/register`
2. **Confirm e‑mail** (link sent to user) → `GET /auth/confirmEmail`
3. **Login** → `POST /auth/login` → receives **access token** + **refresh token** (use cookies)
4. **Refresh token** → `POST /auth/refresh`
5. **Logout / revoke refresh token** → `POST /auth/logout`
6. Optional: manage 2‑factor authentication → `POST /auth/manage/2fa`

All protected endpoints expect the header
`Authorization: Bearer <access token>`.

## 📖 Endpoints quick reference

*(Grouped by tag – see detailed descriptions further below)*

| Tag               | Path prefix(es) | Typical operations                                          |
| ----------------- | --------------- | ----------------------------------------------------------- |
| **Authorization** | `/auth/*`       | User registration, login, tokens, password & 2FA management |
| **Dojo**          | `/Dojo`         | CRUD for Dojo & enroll/unenroll Samurai                     |
| **Kenjutsu**      | `/Kenjutsu`     | CRUD for Kenjutsu styles & assign/unassign Samurai          |
| **Samurai**       | `/Samurai`      | CRUD for Samurai, clan, dojo and kenjutsu relationships     |

---

## 📑 Detailed endpoint list

### Dojo

* **`/Dojo`**
  *Method*: GET
  *Summary*: Lists all Dojo and their regions.
  *Method*: POST
  *Summary*: Creates a Dojo in a given region.
  *Method*: PUT
  *Summary*: Updates a Dojo's name and region given its ID.
* **`/Dojo/{id}`**
  *Method*: GET
  *Summary*: Shows a Dojo with its region and all enrolled Samurai.
  *Method*: DELETE
  *Summary*: Deletes a Dojo.
* **`/Dojo/{id}/samurai`**
  *Method*: POST
  *Summary*: Enrolls multiple Samurai to Dojo given their IDs.
  *Method*: DELETE
  *Summary*: Removes all Samurai from a Dojo.

### Kenjutsu

* **`/Kenjutsu`**
  *Method*: GET
  *Summary*: Lists all Kenjutsu styles.
  *Method*: POST
  *Summary*: Creates a Kenjutsu style.
  *Method*: PUT
  *Summary*: Updates Kenjutsu's style given its ID.
* **`/Kenjutsu/{id}`**
  *Method*: GET
  *Summary*: Shows Kenjutsu and all Samurai that practice it.
  *Method*: DELETE
  *Summary*: Deletes Kenjutsu.
* **`/Kenjutsu/{id}/samurai`**
  *Method*: POST
  *Summary*: Teaches multiple Samurai a Kenjutsu given their IDs.
  *Method*: DELETE
  *Summary*: Unassigns all Samurai from Kenjutsu.
* **`/Kenjutsu/{id}/samurai/partial`**
  *Method*: DELETE
  *Summary*: Unassigns some Samurai from Kenjutsu given their IDs.

### Samurai

* **`/Samurai`**
  *Method*: GET
  *Summary*: Lists all Samurai and their clans.
  *Method*: POST
  *Summary*: Creates a Samurai and puts them in a clan.
  *Method*: PUT
  *Summary*: Updates Samurai's name and clan given their ID.
* **`/Samurai/{id}`**
  *Method*: GET
  *Summary*: Shows Samurai with their clan, what Dojo they're enrolled in, and all their known Kenjutsu.
  *Method*: DELETE
  *Summary*: Deletes Samurai.
* **`/Samurai/{id}/dojo`**
  *Method*: POST
  *Summary*: Enrolls Samurai to a Dojo given its ID.
  *Method*: DELETE
  *Summary*: Removes Samurai from Dojo.
* **`/Samurai/{id}/kenjutsu`**
  *Method*: POST
  *Summary*: Teaches Samurai a Kenjutsu given its ID.
  *Method*: DELETE
  *Summary*: Removes all known Kenjutsu from Samurai.

## 🛠️ Running locally

* **Requirements:** .NET 8 SDK, SQL Server (or SQL LocalDB)
* Clone repo → `dotnet restore` → `dotnet run --project SamuraiApp`
* Browse to `https://localhost:5001/swagger` for interactive docs

### Environment variables

| Variable                     | Purpose                | Example                                               |
| ---------------------------- | ---------------------- | ----------------------------------------------------- |
| `ConnectionStrings__Default` | DB connection string   | `Server=.;Database=SamuraiDb;Trusted_Connection=True` |
| `Jwt__Issuer`                | JWT issuer             | `samurai-api`                                         |
| `Jwt__Audience`              | JWT audience           | `samurai-clients`                                     |
| `Jwt__Key`                   | **Secret** signing key | `SuperSecretKey123!`                                  |

## 🚀 Deploying to Azure App Service

1. Set the same environment variables in the App Service **Configuration** blade
2. Use GitHub Actions or `az webapp up` to deploy
3. Open **Networking → Inbound Traffic** to whitelist IPs if needed

> **Connection note:** If an username uses the @ symbol, you might get error 40532 from Microsoft SQL Server. Add another @ plus the server's name to the end of the username to mitigate.

> **Firewall note:** If you can access `/swagger` but others cannot, check *Access Restrictions* in **Networking** and ensure the rule list allows public traffic or specific IP ranges as required.

## 🧪 Sample cURL snippets

```bash
# Register
curl -X POST ${BASE_URL}/auth/register -H "Content-Type:application/json" \
     -d '{"email":"user@example.com","password":"P@ssw0rd!"}'

# Login
curl -X POST ${BASE_URL}/auth/login -H "Content-Type:application/json" \
     -d '{"email":"user@example.com","password":"P@ssw0rd!"}'
# -> returns accessToken, refreshToken
```

## 📝 To-do
* Update Clan parameter from Samurai into its own class
* Fix `/Samurai/{id}/dojo` DELETE method to properly remove the FK

## 🧾 License

Released under MIT License — see `LICENSE` file.
