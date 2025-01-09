# ConsimpleTask Web API

## Setup

### 1. Clone the Repository

Clone this repository to your local machine using the following command:

git clone https://github.com/elax11/ConsimpleTask.git

### 2. Set up the Database

The database will be automatically created when the application starts if it doesn't already exist. 
However, you need to ensure that the correct connection string is provided.

In appsettings.json, update the DefaultConnection key to point to your SQL Server instance.

### 3. Start the Application

Run the application from Microsoft Visual Studio.

## API Endpoints

### 1. GET /api/shop/birthdays

Returns a list of customers whose birthday matches the specified month and day.

Query Parameters:
month: The month of the birthday (1-12).
day: The day of the birthday (1-31).

Example Request:

GET /api/shop/birthdays?month=5&day=15

###  2. GET /api/shop/recent-buyers

Returns a list of customers (id, name) who have purchased in the last N days. 
For each customer, it displays the date of their last purchase.

Query Parameters:
days: The number of days (positive integer) to filter recent buyers.

Example Request:

GET /api/shop/recent-buyers?days=10

### 3. GET /api/shop/popular-categories

Returns a list of product categories purchased by the found customer.
For each category, the number of units purchased is returned.

Query Parameters:
customerId: The ID of the customer (positive integer).

Example Request:

GET /api/shop/popular-categories?customerId=1

## Database Schema

The following tables are created in the database:

Customers: Stores customer details.
Products: Stores product details.
Purchases: Stores purchase transactions.
PurchaseItems: Stores details of items bought in each purchase.

## Seeding Data

The database is automatically seeded with sample data on application startup. This includes:

50 customers with random names, birthdates, and registration dates.
100 products with random names, categories, articles, and prices.
100 purchases with random customers and products.

## Contact

If you have any questions or suggestions, feel free to reach out to me:

Email: leto.elax@gmail.com


