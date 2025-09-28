Feature: Pet Management
  As a volunteer
  I want to manage pets
  So that I can maintain detailed information about animals in my care

  Background:
    Given the system is running
    And the database is clean
    And I have a volunteer in the system
    And I have species and breeds available

  Scenario: Add a pet to a volunteer
    Given I have pet data with:
      | Field        | Value              |
      | Name         | Buddy              |
      | Description  | Friendly dog       |
      | Color        | Golden             |
      | HealthInfo   | Healthy            |
      | Weight       | 25.5               |
      | Height       | 60.0               |
      | IsSterilized | true               |
      | IsVaccinated | true               |
      | BirthDate    | 2020-01-15         |
      | PetStatus    | LookingForHome     |
    And I have address data:
      | Street  | 123 Main St    |
      | City    | New York       |
      | State   | NY             |
      | ZipCode | 10001          |
    When I add the pet to the volunteer
    Then the pet should be created successfully
    And the pet should be associated with the volunteer
    And the pet should have the provided information


  Scenario: Add pet with invalid data
    Given I have invalid pet data:
      | Field        | Value              |
      | Name         |                    |
      | Description  |                    |
      | Color        |                    |
      | HealthInfo   |                    |
      | Weight       | -5.0               |
      | Height       | -10.0              |
      | IsSterilized | true               |
      | IsVaccinated | true               |
      | BirthDate    | 2030-01-01         |
      | PetStatus    | InvalidStatus      |
    When I add the pet to the volunteer
    Then the pet request should fail with validation errors
    And the pet should not be created

  Scenario: Add pet without required species and breed
    Given I have pet data with:
      | Field        | Value              |
      | Name         | Rex                |
      | Description  | Good dog           |
      | Color        | Brown              |
      | HealthInfo   | Healthy            |
      | Weight       | 30.0               |
      | Height       | 70.0               |
      | IsSterilized | true               |
      | IsVaccinated | true               |
      | BirthDate    | 2019-05-20         |
      | PetStatus    | LookingForHome     |
    But no species and breed are selected
    When I add the pet to the volunteer
    Then the pet request should fail with validation errors
    And the pet should not be created
