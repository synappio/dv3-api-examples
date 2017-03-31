# DV3 RES API example in PHP

Validate an email address using the realtime API:

`php realtime_check.php <api key> <email address>`

Upload a file for validation, wait for the validation to finish and display the summary
(the file must be a CSV file with no headers and the emails must be in the first column):

`php validate_list.php <api key> <list name> <filename>`

Find a full documentation of the DV3 REST API [here](http://docs.datavalidation.apiary.io/).
