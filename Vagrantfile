Vagrant.configure(2) do |config|
    config.vm.box = "ubuntu/xenial32"

    config.vm.provider "virtualbox" do |vb|
        vb.cpus = 1
        vb.memory = 256
        vb.customize ["modifyvm", :id, "--ioapic", "on"]
        vb.customize ["modifyvm", :id, "--nictype1", "Am79C973"]
    end
    config.vm.boot_timeout = 1200

    config.vm.provision "install tarantool",
        type: "shell",
        binary: true,
        keep_color: true,
        inline: "curl http://download.tarantool.org/tarantool/1.6/gpgkey | sudo apt-key add -
release=`lsb_release -c -s`

sudo rm -f /etc/apt/sources.list.d/*tarantool*.list
sudo tee /etc/apt/sources.list.d/tarantool_1_6.list <<- EOF
deb http://download.tarantool.org/tarantool/1.6/debian/ $release main
deb-src http://download.tarantool.org/tarantool/1.6/debian/ $release main
EOF

sudo apt-get update
sudo apt-get -y install tarantool dos2unix"

    config.vm.provision "copy lua config",
        type: "shell",
        binary: true,
        keep_color: true,
        inline: "mkdir -p /opt/tarantool && cp /vagrant/tarantool.lua /opt/tarantool/tarantool.lua && dos2unix /opt/tarantool/tarantool.lua"

    config.vm.provision "run tarantool",
        type: "shell",
        binary: true,
        keep_color: true,
        inline: "tarantool /opt/tarantool/tarantool.lua"

    config.vm.network "forwarded_port", guest: 3301, host: 3301

end