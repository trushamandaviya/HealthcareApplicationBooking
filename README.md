# HealthcareApplicationBooking
This project is a Healthcare Appointment Booking API built using .NET 8, Entity Framework Core, and MySQL. The API includes user authentication using JWT tokens, management of healthcare professionals, and appointment booking.

Features

    JWT Authentication for secure access.
    Healthcare Professional Management: View professionals and their specializations.
    Appointment Management: Book, view, and cancel appointments.
    Swagger Integration: Interactive API documentation for easy testing.

Prerequisites

Before you start, ensure you have the following installed:

    .NET 8 SDK
    MySQL Server
    MySQL Workbench (optional, for database management)
    Postman or cURL for testing APIs (optional)

Setup Instructions
1. Clone the Repository

git clone <repository-url>
cd HealthcareAppointmentBooking

2. Configure the Database
Create the Database

    Open MySQL Workbench or connect to your MySQL server.
    Create the database:

        CREATE DATABASE healthcare_appointment;

Create Tables

Run the following SQL commands to create the required tables:

      CREATE TABLE `healthcare_appointment`.`users` (
        `user_id` INT NOT NULL AUTO_INCREMENT,
        `name` VARCHAR(45) NOT NULL,
        `email` VARCHAR(255) NULL,
        PRIMARY KEY (`user_id`))
      COMMENT = 'Users who can book appointment';
      
      CREATE TABLE `healthcare_appointment`.`specialities` (
        `speciality_id` INT NOT NULL AUTO_INCREMENT,
        `speciality_name` VARCHAR(45) NOT NULL,
        PRIMARY KEY (`speciality_id`))
      COMMENT = 'Specialities of the healthcare practisners';
      
      CREATE TABLE `healthcare_appointment`.`healthcare_professionals` (
        `professional_id` INT NOT NULL AUTO_INCREMENT,
        `professional_name` VARCHAR(45) NOT NULL,
        PRIMARY KEY (`professional_id`))
      COMMENT = 'Doctors who will provide the treatment to the users';
      
      CREATE TABLE `healthcare_appointment`.`professionals_specialities` (
        `professionals_specialities_id` INT NOT NULL AUTO_INCREMENT,
        `healthcare_professional_id` INT NOT NULL,
        `speciality_id` INT NOT NULL,
        PRIMARY KEY (`professionals_specialities_id`),
        INDEX `FK_healthcare_professional_specialities_idx` (`healthcare_professional_id` ASC) VISIBLE,
        INDEX `FK_professionals_specialities_specialities_idx` (`speciality_id` ASC) VISIBLE,
        CONSTRAINT `FK_professionals_specialities_healthcare_professionals`
          FOREIGN KEY (`healthcare_professional_id`)
          REFERENCES `healthcare_appointment`.`healthcare_professionals` (`professional_id`)
          ON DELETE CASCADE
          ON UPDATE NO ACTION,
        CONSTRAINT `FK_professionals_specialities_specialities`
          FOREIGN KEY (`speciality_id`)
          REFERENCES `healthcare_appointment`.`specialities` (`speciality_id`)
          ON DELETE CASCADE
          ON UPDATE NO ACTION)
      COMMENT = 'Connector table for the healthcare professional and his specialities';
      
      CREATE TABLE `healthcare_appointment`.`appointment_status` (
        `appointment_status_id` INT NOT NULL AUTO_INCREMENT,
        `status` VARCHAR(45) NOT NULL,
        PRIMARY KEY (`appointment_status_id`))
      COMMENT = 'Represents status of the appointment';
      
      CREATE TABLE `healthcare_appointment`.`appointment` (
        `appointment_id` INT NOT NULL AUTO_INCREMENT,
        `user_id` INT NOT NULL,
        `healthcare_professional_id` INT NOT NULL,
        `start_time` DATETIME NOT NULL,
        `end_time` DATETIME NOT NULL,
        `status_id` INT NULL,
        PRIMARY KEY (`appointment_id`),
        INDEX `FK_appointment_users_idx` (`user_id` ASC) VISIBLE,
        INDEX `FK_appointment_healthcare_professionals_idx` (`healthcare_professional_id` ASC) VISIBLE,
        INDEX `FK_appointment_appointment_status_idx` (`status_id` ASC) VISIBLE,
        CONSTRAINT `FK_appointment_users`
          FOREIGN KEY (`user_id`)
          REFERENCES `healthcare_appointment`.`users` (`user_id`)
          ON DELETE CASCADE
          ON UPDATE NO ACTION,
        CONSTRAINT `FK_appointment_healthcare_professionals`
          FOREIGN KEY (`healthcare_professional_id`)
          REFERENCES `healthcare_appointment`.`healthcare_professionals` (`professional_id`)
          ON DELETE CASCADE
          ON UPDATE NO ACTION,
        CONSTRAINT `FK_appointment_appointment_status`
          FOREIGN KEY (`status_id`)
          REFERENCES `healthcare_appointment`.`appointment_status` (`appointment_status_id`)
          ON DELETE NO ACTION
          ON UPDATE NO ACTION)
      COMMENT = 'Main table which has data of the appointments of the user with the professionals';
      
      ALTER TABLE `healthcare_appointment`.`users` 
      ADD COLUMN `password` VARCHAR(255) NOT NULL AFTER `email`;
      
      ALTER TABLE `healthcare_appointment`.`users` 
      CHANGE COLUMN `email` `email` VARCHAR(255) NOT NULL ,
      ADD UNIQUE INDEX `email_UNIQUE` (`email` ASC) VISIBLE;

3. Update Configuration
Edit appsettings.json

Update the database connection and JWT settings in the HC.API project:

    "ConnectionStrings": {
      "DefaultConnection": "server=localhost;port=3306;database=healthcare_appointment;user=root;password=yourpassword"
    },
    
    "JwtSettings": {
      "Key": "YourSuperSecureKeyHere",
      "Issuer": "https://localhost:7150",
      "Audience": "https://localhost:7150",
      "ExpiryMinutes": 60
    }

4. Build and Run the Project

Open Swagger UI to explore the APIs:

      https://localhost:7150/swagger/index.html

Testing the API
1. Register a User

    Endpoint: POST /api/Account/Register
    Request Body:

        {
          "name": "Trusha Savsani",
          "email": "trusha.savsani@gmail.com",
          "password": "Admin@1"
        }

Response:

    {
      "token": "<YourJWTToken>"
    }
    
2. Login and Get JWT Token

    Endpoint: POST /api/Account/Login
    Request Body:
   
        {
          "email": "john.doew@example.com",
          "password": "password123"
        }

Response:

    {
      "token": "<YourJWTToken>"
    }

3. Get Healthcare Professionals

    Endpoint: POST /api/Healthcare/GetHealthcareProfessionals
    Authorization Header:

    Authorization: Bearer <YourJWTToken>

    Response:

         [
          {
            "professionalId": 1,
            "name": "Dr. John Bush",
            "specialities": [
              "Cardiology",
              "Neurology"
            ]
          },
          {
            "professionalId": 2,
            "name": "Dr. Jane Smith",
            "specialities": [
              "Dermatology"
            ]
           }
        ]

4. Book an Appointment

    Endpoint: POST /api/Healthcare/BookAppointment
    Request Body:

        {
            "userId": 1,
            "professionalId": 2,
            "startTime": "2024-11-17T10:00:00Z",
            "endTime": "2024-11-17T11:00:00Z"
        }

   Response:
       {
          "message": "Appointment booked successfully!"
        }

5. Cancel an Appointment

    Endpoint: POST /api/Healthcare/CancelAppointment
    Request Body:

        {
          "userId": 2,
          "appointmentId": 4
        }

   Response:
       {
          "message": "Appointment cancelled successfully."
        }

6. List Appointments for user

Endpoint: POST /api/Healthcare/User/{userId}/Appointments
   
Request:
      Replace userid with your id

Response:
        [
                  {
                    "appointmentId": 1,
                    "professionalName": "Dr. Jane Smith",
                    "status": "Booked",
                    "startTime": "2024-11-17T10:00:00",
                    "endTime": "2024-11-17T11:00:00"
                  },
                  {
                    "appointmentId": 2,
                    "professionalName": "Dr. Emily Brown",
                    "status": "Booked",
                    "startTime": "2024-11-17T06:00:00",
                    "endTime": "2024-11-17T11:00:00"
                  }
                ]

Query to add dummy data into tables:

    INSERT INTO `healthcare_appointment`.`specialities` (`speciality_name`) VALUES
    ('Cardiology'),
    ('Dermatology'),
    ('Neurology'),
    ('Pediatrics'),
    ('Orthopedics');
    
    INSERT INTO `healthcare_appointment`.`healthcare_professionals` (`professional_name`) VALUES
    ('Dr. John Bush'),
    ('Dr. Jane Smith'),
    ('Dr. Emily Brown'),
    ('Dr. Michael Johnson'),
    ('Dr. Sarah Davis');
    
    INSERT INTO `healthcare_appointment`.`professionals_specialities` (`healthcare_professional_id`, `speciality_id`) VALUES
    (1, 1), -- Dr. John Bush is a Cardiologist
    (2, 2), -- Dr. Jane Smith is a Dermatologist
    (3, 3), -- Dr. Emily Brown is a Neurologist
    (4, 4), -- Dr. Michael Johnson is a Pediatrician
    (5, 5); -- Dr. Sarah Davis is an Orthopedist
    
    INSERT INTO `healthcare_appointment`.`professionals_specialities` (`healthcare_professional_id`, `speciality_id`) VALUES (3, 1);
    
    INSERT INTO `healthcare_appointment`.`appointment_status` (`status`) VALUES
    ('Booked'),
    ('Completed'),
    ('Cancelled');

Additional Notes

    Logging:
        Debugging logs for JWT validation are enabled in Program.cs:

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Authentication failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token validated for user: " + context.Principal.Identity.Name);
            return Task.CompletedTask;
        }
    };

Security:

    Use a strong secret for JwtSettings.Key.
    Use HTTPS in production.
    Store sensitive data (e.g., connection strings) in environment variables or a secure vault.
