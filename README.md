NAntFind
========

	<property name="find.module.path" value="find;dependency/more/find" />

    <find package="SQLServer" required="true" version="100" />
    <echo message="${SQLServer.found}" />
    <echo message="${SQLServer}" />

    <find file="SQLCmd.exe" package="SQLServer" version="100" recursive="true"/>
    <echo message="${SQLCmd.exe.found}" />
    <echo message="${SQLCmd.exe}" />

NAntFind is a CMake find style dependency discovery extension for NAnt.

The FindSQLServer.include

    <package name="VisualStudio" default="11.0">
        <version value="11.0">
        	<hints>
				<hint key="HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\11.0" name="InstallDir" />
    		</hints>
    		<names>
    			<name value="devenv.exe" />
    			<name value="tf.exe" />
    		</names>
    	</version>
    	<version value="10.0">
    		<hints>
    			<hint value="C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE" />
    			<hint value="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE" />
				<hint key="HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\10.0" name="InstallDir" />
				<hint key="HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0" name="InstallDir" />
    		</hints>
    		<names>
    			<name value="devenv.exe" />
    			<name value="tf.exe" />
    		</names>
    	</version>
    </package>

The file name must be _Find_ + <package name> + _.xml_, so NAntFind can locate your find module automatically.

----
Feature
----
* Find packages
* Find one file in a given package
* Search for find modules in ${find.module.path}
* Search for a specific version
* Default package version can be specified
* Query value from registry

Result can be queried in NAnt variables:
* **${package.found}**: True/False
* **${package}**: Package path
* **${package.version}**: Package version.

----
TODO
----
* Allow substution in find module
* Use environment variables in find module
* A more flexible and easy to use DSL

----
License
----
[GNU Lesser General Public License][1]


  [1]: http://www.gnu.org/copyleft/lgpl.html
