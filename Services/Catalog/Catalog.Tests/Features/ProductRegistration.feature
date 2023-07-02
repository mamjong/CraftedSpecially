Feature: Product registration

    To register a new product in the shop, a user must provide the following information:
    - name
    - description

    Rule: Products must have a unique and valid name and description

        Scenario Outline: Register a product without valid information
            Registering a product cannout be done with invalid information

            When a product is registered with name "<name>" and description "<description>"
            Then an exception is provided with the information that the "<invalidField>" is invalid
            And the meter of products registered is increased by 1

            Examples:
                | name        | description     | invalidField      |
                |             |                 | name, description |
                | TestProduct |                 | description       |
                |             | TestDescription | name              |

        Scenario: Register a product with valid information

            When a product is registered with valid information
            Then a product registered event occurs
            And the meter of products registered is increased by 1


