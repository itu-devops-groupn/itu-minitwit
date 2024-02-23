# Setting up Vagrant and VirtualBox (Mac/intel)

## Follow installation guide

https://github.com/itu-devops/lecture_notes/blob/master/sessions/session_03/README_PREP.md

Most of it was straight forward and worked as explained. I used brew install for it, following this guide:
https://www.itu.dk/people/ropf/blog/vagrant_install.html 

## Setup SSH access to digitalOcean

1. Register on digital ocean
2. Create token access via this guide https://docs.digitalocean.com/products/droplets/how-to/add-ssh-keys/to-team/
   1. **Important** - Use the curl option and _NOT_ the CLI option for uploading the public SSH-key. The first 3 steps of the CLI guide worked fine for me, but the last step failed.
3. Once everything is uploaded, create the env variables for the vagrant file to recognize. Open `~/.bash_profile` and add the following lines:
   1.  `export DIGITAL_OCEAN_TOKEN="your_token_here"`
   2.  `export SSH_KEY_NAME="your_ssh_key_name_here"` - This is just the name of the SSH key you uploaded to digital ocean, can be found under _settings -> security -> SSH keys_ in case you forgot.
   3.  Make sure to run `source ~/.bash_profile` to apply the changes for your current terminal session.
4.  Everything should work now.



