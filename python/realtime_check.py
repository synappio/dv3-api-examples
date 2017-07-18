#!/usr/bin/env python3
# coding: utf-8
from __future__ import absolute_import, division, print_function
import logging
import sys

import dv3_api

logging.basicConfig(level=logging.INFO)
dv3_api.KEY, email = sys.argv[1:]  # pylint: disable=unbalanced-tuple-unpacking

grade = dv3_api.realtime_check(email)
logging.info('%s got a %s.', email, grade)
