# API Documentation

## Login

- Who?: Customers and Bakers
- Type: POST
- Body:
  - ac_username: The username to log in as.
  - password: The password for the username
- Reponse: "true" if success, "false" if not.


Datenbanken:
Tables:
-user: (id, name, type (baker OR customer)

API-Abfragen:
Pizza bestellen - POST - /api/createOrder - List<Pizza>
Verfügbare Pizzen einsehen - GET - /api/listPizzas
Verfügbare Zutaten abfragen - GET - /api/listIngredients

Login - POST - /api/login - username: <Benutzername>, password: <Passwort>
Account erstellen - POST - /api/createAccount - newUsername: <Neuer Benutzername>, newPassword: <Neues Passwort>, newRole: <baker ODER customer>
