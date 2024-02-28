# -*- mode: ruby -*-
# vi: set ft=ruby :

$ip_file = "db_ip.txt"

Vagrant.configure("2") do |config|
  config.vm.box = 'digital_ocean'
  config.vm.box_url = "https://github.com/devopsgroup-io/vagrant-digitalocean/raw/master/box/digital_ocean.box"
  config.ssh.private_key_path = '~/.ssh/id_rsa'
  config.vm.synced_folder "." , "/vagrant", type: "rsync"

  config.vm.define "webserver", primary: true do |server|
    server.vm.provider :digital_ocean do |provider|
      provider.ssh_key_name = ENV["SSH_KEY_NAME"]
      provider.token = ENV["DIGITAL_OCEAN_TOKEN"]
      provider.image = 'ubuntu-22-04-x64'
      provider.region = 'fra1'
      provider.size = 's-1vcpu-1gb'
      provider.privatenetworking = true
    end

    server.vm.hostname = "webserver"

    server.vm.provision "shell", privileged: false, inline: <<-SHELL
        echo "Installing Docker..."

        sudo apt-get update -qq
        sudo apt install -qq -y docker.io
        sudo apt install -qq -y docker-compose-v2

        echo "Docker finished installing"
        echo "Printing Docker version..."
        docker --version

        echo "Loading docker image..."
        docker load -i /vagrant/remote_files/minitwit_web.tar
        docker load -i /vagrant/remote_files/minitwit_api.tar

        echo "Starting the application..."
        docker compose -f /vagrant/docker-compose.yml up -d
    SHELL
    end
end