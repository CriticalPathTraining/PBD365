# 'dataset' holds the input data for this script

ageGroups = c(0,17,29,39,49,64,100)
ageGroupLabels = c("Under 18", "18 to 29", "30 to 39", "40 to 49", "50 to 64", "65 and up")

dataset$AgeGroup <- cut(dataset$Age, breaks = ageGroups, labels = ageGroupLabels)

output <- dataset