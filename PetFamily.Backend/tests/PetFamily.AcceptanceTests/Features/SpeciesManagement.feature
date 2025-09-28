Feature: Species Management
  As a system administrator
  I want to manage species and breeds
  So that I can maintain a catalog of animal types and their breeds

  Background:
    Given the system is running
    And the database is clean

  Scenario: Get all species with breeds
    Given I have 3 species in the system:
      | Species Name | Breeds                    |
      | Dog          | Labrador, Golden Retriever |
      | Cat          | Persian, Maine Coon       |
      | Bird         | Canary, Parakeet          |
    When I request all species
    Then I should receive 3 species
    And each species should include their breeds
    And the species should be ordered alphabetically

  Scenario: Get species with specific breeds
    Given I have a species "Dog" with breeds:
      | Breed Name        |
      | Labrador          |
      | Golden Retriever  |
      | German Shepherd   |
      | Bulldog           |
    When I request all species
    Then I should receive the "Dog" species
    And it should have 4 breeds
    And the breeds should include "Labrador" and "Golden Retriever"

  Scenario: Get species with no breeds
    Given I have a species "Fish" with no breeds
    When I request all species
    Then I should receive the "Fish" species
