@UI
Feature: Login

   Scenario Outline: Login with different users
    Given the user enters "<username>" and "<password>"
    When they press login
    Then they should see the homepage

    Examples:
      | username | password | result           |
      | demo     | demo123  | dashboard        |
      | admin    | wrong    | error message    |
      | test     | test123  | dashboard        |