#!/bin/bash
set -ue

flake8 *.py
pylint *.py
