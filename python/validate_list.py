#!/usr/bin/env python3
# coding: utf-8
from __future__ import absolute_import, division, print_function
import logging
import os
import sys
import time

import dv3_api

logging.basicConfig(level=logging.INFO)
dv3_api.KEY = sys.argv[1]

list_name, file_path = sys.argv[2:4]  # pylint: disable=unbalanced-tuple-unpacking
assert os.path.isfile(file_path)

logging.info('Getting upload URL...')
upload_url = dv3_api.get_upload_url(list_name, email_column=0, has_header=False)
logging.info('Upload URL: %s', upload_url)
logging.info('Uploading file contents')
list_id = dv3_api.upload(upload_url, file_path)
logging.info('List created. Id: %s', list_id)

while True:
    time.sleep(5)
    info = dv3_api.get_list_info(list_id)
    if info['status_value'] == 'VALIDATED':
        logging.info('List validated. Grade summary: %s', info['grade_summary'])
        break
    elif info['status_value'] == 'FAILED':
        logging.info('List validation failed!')
        break

    logging.info('Status: %s (%.2f%%)', info['status_value'], info['status_percent_complete'])
