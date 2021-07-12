using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace AtomicCore.WebService.RequestAgent
{
    /// <summary>
    /// NetCore Startup
    /// </summary>
    public class Startup
    {
        #region Constructor

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configuration">ϵͳ����</param>
        /// <param name="env">WebHost����</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        #endregion

        #region Propertys

        /// <summary>
        /// ϵͳ����
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// WebHost����
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }

        #endregion

        #region Public Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region AtomicCore�����ʼ��

            AtomicCore.AtomicKernel.Initialize();

            #endregion

            #region ���л�������Linux or IIS��

            //���������linuxϵͳ�ϣ���Ҫ������������ã�
            //services.Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true);
            //���������IIS�ϣ���Ҫ������������ã�
            //services.Configure<IISServerOptions>(options => options.AllowSynchronousIO = true);

            #endregion

            #region ���ض�ȡ�����AppSettings��



            #endregion

            #region �����ϴ�������Ʒ�ֵ���޸�Ĭ�Ϸ�ֵ��

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            #endregion

            #region Session��IHttpContextAccessor��MVC��Cookie

            /* ������ģʽ -> ���ǵ���ģʽ,��ʼrazor����ʱ����ģʽ�� */
            if (this.WebHostEnvironment.IsDevelopment())
                services.AddRazorPages().AddRazorRuntimeCompilation();

            /* ������ͨ���м������ */
            services.AddMemoryCache();                                      // ʹ��MemoryCache�м��������������ʹ�� IMemoryCache �ӿڣ�
            services.AddSession();                                          // ʹ��Session��ʵ��֤����Ҫ������ Mvc ֮ǰ��
            services.AddHttpContextAccessor();                              // ע�ᵱǰ�߳�ȫ�������������Ľӿڵ���
            services.AddOptions();                                          // ����Optionsģʽ

            /* ��HtmlEncoder���á� */
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));   // ����HTML�������ʵ��
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // ע��Encoding����

            /* �����ݱ����� */
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(
                this.WebHostEnvironment.ContentRootPath +
                Path.DirectorySeparatorChar +
                "DataProtection"
            ));

            /*
             * ��MVC����м����
             * 1.ע��MVC����������ͼ
             * 2.֧��NewtonsoftJson
             * 3.����MVC�汾
             */
            services.AddControllersWithViews(options =>
            {
                /*
                *  1.����ȫ������(������̨����������)
                *  2.�޸Ŀ�����ģ�Ͱ󶨹���
                */
                //options.Filters.Add<BizPermissionTokenAttribute>();
                //options.ModelMetadataDetailsProviders.Add(new BizModelBindingMetadataProvider());
            })
            .AddNewtonsoftJson(options =>
            {
                /*
                *  ֧�� NewtonsoftJson
                *  ����ĸ��д -> DefaultContractResolver
                *  ����ĸСд -> CamelCasePropertyNamesContractResolver
                */
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            /*
             * ��Cookie����м����
             * 1.����cookie����
             * 2.Add CookieTempDataProvider after AddMvc and include ViewFeatures.
             */
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.SameAsRequest;
            });
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* ����DEBUGģʽ */
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            /* ���̬��Դ����(����ģʽ������Cache) */
            FileServerOptions options = new FileServerOptions();
            options.StaticFileOptions.OnPrepareResponse = SetCacheControl;
            app.UseFileServer(options);

            /* ��˳����������Mvc����м�� */
            app.UseSession();                               //����Session
            app.UseRouting();                               //����Routing
            //app.UseAuthorization();                         //������֤����
            app.UseResponseCaching();                       //�����������
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ����cache control
        /// </summary>
        /// <param name="context"></param>
        private static void SetCacheControl(StaticFileResponseContext context)
        {
            int second = 365 * 24 * 60 * 60;
            context.Context.Response.Headers.Add("Cache-Control", new[] { "public,max-age=" + second });
            context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
        }

        #endregion
    }
}