workspace {
    model {
        client = person "Client"
        owner = person "Owner"
        
        softwareSystem = softwareSystem "Software System" {
            webapp = container "Web Aplication" {
                client -> this "Uses"
                owner -> this "Uses"
                
                api = component "API"    
                domain = component "Domain"
                projections = component "Projections"
                authentication = component "Authentication"
            }

            eventstore = container "Event Store" {
                webapp -> this "Reads from and writes to"
                
                domain -> this "Saves events to"
                projections -> this "Reads events from"
            }
            
            container "Configuration Database" {
                webapp -> this "Reads from and writes to"
                
                authentication -> this "Reads and writes users data"
            }
        }
        
        client -> api "Uses"
        owner -> api "Uses"
        
        api -> domain "Publishes commands"
        api -> projections "Queries"
        api -> authentication "Asks to authorize"
        
        projections -> domain "Consumes events from"
    }

    views {
        systemContext softwareSystem {
            include *
            autolayout lr
        }

        container softwareSystem {
            include *
            autolayout lr
        }
        
        component webapp {
            include *
            autolayout lr
        }

        theme default
    }
}