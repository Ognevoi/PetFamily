Feature: Volunteer Management
  As a system administrator
  I want to manage volunteers
  So that I can maintain a database of people who help with pet care and adoption

  Background:
    Given the system is running
    And the database is clean

  Scenario: Create a new volunteer
    Given I have volunteer data with:
      | Field           | Value                    |
      | FirstName       | John                     |
      | LastName        | Doe                      |
      | Email           | john.doe@example.com     |
      | Description     | Experienced pet lover    |
      | ExperienceYears | 5                        |
      | PhoneNumber     | +1234567890              |
    When I create a volunteer
    Then the volunteer should be created successfully
    And the volunteer should have the provided information
    And the volunteer should be retrievable by ID

  Scenario: Create a volunteer with social networks
    Given I have volunteer data with:
      | Field           | Value                    |
      | FirstName       | Jane                     |
      | LastName        | Smith                    |
      | Email           | jane.smith@example.com  |
      | Description     | Social media savvy       |
      | ExperienceYears | 3                        |
      | PhoneNumber     | +1987654321              |
    And I add social networks:
      | Platform | URL                           |
      | Facebook | https://facebook.com/jane     |
      | Twitter  | https://twitter.com/jane      |
    When I create a volunteer
    Then the volunteer should be created successfully
    And the volunteer should have the social networks

  Scenario: Create a volunteer with assistance details
    Given I have volunteer data with:
      | Field           | Value                    |
      | FirstName       | Bob                      |
      | LastName        | Johnson                  |
      | Email           | bob.johnson@example.com |
      | Description     | Veterinary background    |
      | ExperienceYears | 10                       |
      | PhoneNumber     | +1555123456              |
    And I add assistance details:
      | Type        | Description                    |
      | Medical Care| Provide medical assistance     |
      | Training    | Help with pet training         |
    When I create a volunteer
    Then the volunteer should be created successfully
    And the volunteer should have the assistance details

  Scenario: Get all volunteers
    Given I have 3 volunteers in the system
    When I request all volunteers
    Then I should receive 3 volunteers
    And each volunteer should have basic information


  Scenario: Create volunteer with invalid data
    Given I have invalid volunteer data:
      | Field           | Value                    |
      | FirstName       |                         |
      | LastName        |                         |
      | Email           | invalid-email           |
      | Description     |                         |
      | ExperienceYears | -1                      |
      | PhoneNumber     | invalid-phone            |
    When I create a volunteer
    Then the volunteer request should fail with validation errors
    And the volunteer should not be created
