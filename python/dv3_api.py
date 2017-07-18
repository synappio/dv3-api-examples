# coding: utf-8
from __future__ import absolute_import, division, print_function
import requests


# this needs to be set before API calls can be made
KEY = None


_API_ROOT = 'https://dv3.datavalidation.com/api/v2'


def _getAuthHeaders():
    return {'Authorization': 'bearer %s' % KEY}


def _get(url, url_params=None):
    url = _API_ROOT + url
    if url_params:
        url = url + '?' + '&'.join(
            '%s=%s' % (requests.utils.quote(str(k)), requests.utils.quote(str(v)))
            for k, v in url_params.items()
        )
    r = requests.get(url, headers=_getAuthHeaders())
    r.raise_for_status()
    return r.json()


def get_upload_url(list_name, email_column, has_header):
    return _get(
        '/user/me/list/create_upload_url/',
        {'name': list_name, 'email_column_index': int(email_column), 'has_header': bool(has_header)}
    )


def upload(upload_url, file_path):
    with open(file_path, 'rb') as f:
        r = requests.post(upload_url, files={'file': f}, headers=_getAuthHeaders())
    r.raise_for_status()
    return r.json()


def get_list_info(list_id):
    return _get('/user/me/list/%s/' % list_id)


def realtime_check(email):
    result = _get('/realtime/', {'email': email})
    assert result['status'] == 'ok'
    return result['grade']
