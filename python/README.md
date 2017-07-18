# DV3 REST API example in Python 2/3

Validate an email address using the realtime API:

`python realtime_check.py <api key> <email address>`

Upload a file for validation, wait for the validation to finish and display the summary
(the file must be a CSV file with no headers and the emails must be in the first column):

`python validate_list.py <api key> <list name> <filename>`

Find a full documentation of the DV3 REST API [here](http://docs.datavalidation.apiary.io/).

The samples are compatible with both Python 2.7 and 3+ and require the [requests](http://docs.python-requests.org/en/master/user/install/) library.
