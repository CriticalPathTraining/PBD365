barplot(dataset$Sales,        
        names.arg = dataset$AgeGroup, 
        ylab = "Sales Revenue",
        main = "Sales by Customer Age Group", 
        col = c("red", "yellow","orange",  "green", "blue")) 
