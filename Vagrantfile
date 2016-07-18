Vagrant.configure(2) do |config|
    config.vm.box = "ubuntu/trusty32"

    config.vm.provider "virtualbox" do |vb|
        vb.cpus = 1
        vb.memory = 256
        vb.customize ["modifyvm", :id, "--ioapic", "on"]
        vb.customize ["modifyvm", :id, "--nictype1", "Am79C973"]
    end
    config.vm.boot_timeout = 1200
    config.vm.hostname = "ubuntu-trusty"

    config.vm.provision "install tarantool",
        type: "shell",
        binary: true,
        keep_color: true,
        inline: "sudo apt-get update && sudo apt-get -y install tarantool dos2unix"

    config.vm.provision "create tarantool config directiory",
        type: "shell",
        binary: true,
        keep_color: true,
        inline: "mkdir -p /opt/tarantool/work_dir && chmod 0777 /opt/tarantool",
        run: "always"

    config.vm.provision "copy tarantool config",
        type: "file",
        source: "tarantool.lua",
        destination: "/opt/tarantool/tarantool.lua",
        run: "always"

    config.vm.provision "fix newlines, if any problems",
        type: "shell",
        binary: true,
        keep_color: true,
        inline: "dos2unix /opt/tarantool/tarantool.lua",
        run: "always"

    config.vm.provision "fix missing libbfd problem",		
        type: "shell",		
        binary: true,		
        keep_color: true,		
        inline: "sudo ln -s /usr/lib/i386-linux-gnu/libbfd-2.26.1-system.so /usr/lib/i386-linux-gnu/libbfd-2.26-system.so",		
        run: "always"

    config.vm.provision "run tarantool",
        type: "shell",
        binary: true,
        keep_color: true,
        inline: "tarantool /opt/tarantool/tarantool.lua daemon",
        run: "always"

    config.vm.network "forwarded_port", guest: 3301, host: 3301
end