# WPF Games and User Management Application

Overview
This project is a WPF (Windows Presentation Foundation) application developed in C# that incorporates a variety of features, including multiple games, country information, user management, and task management. The application is designed to provide a user-friendly interface for interacting with different functionalities, utilizing data from the REST Countries API.

Features
Games:
The application includes 6 different game projects, each providing a unique gaming experience.
Country Information:
An interface to display and interact with country data sourced from the REST Countries API.
User Management:
View a list of existing users.
Create new users.
Task Management:
View a list of existing tasks.
Create new tasks.
Project Structure
The project is organized into multiple components:

Games: Contains 9 different WPF projects, each representing a different game.
Country Information: Includes pages for displaying and interacting with country data.
User Management: Features pages for user management operations.
Task Management: Includes pages for managing tasks.
Getting Started
Prerequisites
.NET 8.0
Visual Studio 2022
Installation
Clone the repository:

bash
Copy code
git clone https://github.com/mataneeman/ProjectWpf.git
Open the solution in Visual Studio:

Navigate to the solution directory.
Open the .sln file in Visual Studio.
Restore NuGet packages:

Right-click on the solution in Solution Explorer.
Select "Restore NuGet Packages".
Build and Run:

Press F5 or click on "Start" to build and run the application.
Usage
Games
Navigate to the "Games" section from the main menu.
Select a game from the list to start playing.
Country Information
Navigate to the "Countries" section.
View and interact with country details using the API.
User Management
Navigate to the "Users" section.
View Users: Browse the list of existing users.
Create User: Click on the "Create New User" button to add a new user.
Task Management
Navigate to the "Tasks" section.
View Tasks: Browse the list of existing tasks.
Create Task: Click on the "Create New Task" button to add a new task.
API Integration
The country information is retrieved from the REST Countries API.
API requests are handled within the Countries class (https://restcountries.com/v3.1/all).

Thank you for Watching

